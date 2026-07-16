using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaExportation;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaSerialization;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;
using Kotor.DevelopmentKit.AreaDesigner.Settings;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.Renderers.Descriptors;
using Kotor.NET.Resources.KotorARE;
using Kotor.NET.Resources.KotorBWM;
using Kotor.NET.Resources.KotorERF;
using Kotor.NET.Resources.KotorGFF;
using Kotor.NET.Resources.KotorIFO;
using Kotor.NET.Resources.KotorLYT;
using Kotor.NET.Resources.KotorMDL;
using Kotor.NET.Resources.KotorMDL.Nodes;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.ViewModels;

public class AreaDesignerViewModel : ReactiveObject
{
    public Interaction<Unit, Point> GetMousePoint = new();
    public Interaction<Unit, WorldObjectTemplate?> SelectTileTemplate = new();
    public Interaction<Unit, string?> SelectSaveFilepathForArea = new();
    public Interaction<Unit, string?> SelectLoadFilepathForArea = new();
    public Interaction<Unit, Unit> PromptEditSettings = new();
    public Interaction<Unit, Unit> ClearSelection = new();
    public Interaction<WorldObject, Unit> AddToSelection = new();

    public bool IsMode_AddTile => Mode is AddTileMode;
    public bool IsMode_SelectObject => Mode is SelectObjectMode;
    public bool IsMode_AddObject => Mode is AddObjectMode;

    public ObservableCollection<KitItem> Kits { get; } = new();

    public ObservableCollection<object> SelectedPieces
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public WorldObject? ActiveWorldObject
    {
        get;
        set
        {
            Mode.SelectedWorldObject = value;
            this.RaiseAndSetIfChanged(ref field, value);
        }
    }

    public AreaScene Scene
    {
        get => Engine.Scene as AreaScene;
    }
    public Area Area
    {
        get => Scene.Area;
        set
        {
            Scene.Area = value;
            this.RaisePropertyChanged(nameof(Area));
        }
    }
    public GLEngine Engine
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public BaseMode Mode
    {
        get;
        private set
        {
            this.RaiseAndSetIfChanged(ref field, value);
            Scene.Mode = value;
        }
    }
    public DesignerSettings Settings { get; } = new();

    // move to settings
    public bool ShowWalls
    {
        get;
        set
        {
            Scene?.DoRenderWalls = value;
            this.RaiseAndSetIfChanged(ref field, value);
        }
    } = true;
    public bool ShowDoors
    {
        get;
        set
        {
            Scene?.DoRenderDoors = value;
            this.RaiseAndSetIfChanged(ref field, value);
        }
    } = true;
    public bool ShowFloors
    {
        get;
        set
        {
            Scene?.DoRenderFloor = value;
            this.RaiseAndSetIfChanged(ref field, value);
        }
    } = true;
    public bool ShowCeilings
    {
        get;
        set
        {
            Scene?.DoRenderCeiling = value;
            this.RaiseAndSetIfChanged(ref field, value);
        }
    } = false;
    public bool ShowCorners
    {
        get;
        set
        {
            Scene?.DoRenderCorners = value;
            this.RaiseAndSetIfChanged(ref field, value);
        }
    } = true;
    public bool ShowObjects
    {
        get;
        set
        {
            Scene?.DoRenderObjects = value;
            this.RaiseAndSetIfChanged(ref field, value);
        }
    } = true;

    public AreaDesignerViewModel()
    {
        Kit.Manager.Refresh();
        Kits = new(Kit.Manager.Kits.Select(x => new KitItem(x)));
        SelectedPieces = new();

        ClearSelection.RegisterHandler(async interaction =>
        {
            ActiveWorldObject = null;
            SelectedPieces.Clear();
            interaction.SetOutput(Unit.Default);
        });

        AddToSelection.RegisterHandler(async interaction =>
        {
            ActiveWorldObject = interaction.Input;
            SelectedPieces.Add(ActiveWorldObject);
            interaction.SetOutput(Unit.Default);
        });

        this.WhenAnyValue(x => x.Mode)
            .Subscribe(mode =>
            {
                this.RaisePropertyChanged(nameof(IsMode_AddTile));
                this.RaisePropertyChanged(nameof(IsMode_AddObject));
                this.RaisePropertyChanged(nameof(IsMode_SelectObject));
            });
    }

    public void SetSceneMode_AddTile()
    {
        Mode = new AddTileMode(Engine, Area, Kits, ActiveWorldObject, Settings);
    }
    public void SetSceneMode_AddObject()
    {
        Mode = new AddObjectMode(Engine, Area, Kits, ActiveWorldObject, Settings);
    }
    public void SetSceneMode_SelectObject()
    {
        Mode = new SelectObjectMode(Engine, Area, Kits, ActiveWorldObject, Settings)
        {
            AddToSelection = AddToSelection,
            ClearSelection = ClearSelection,
        };
    }

    public void ReloadKit(string filepath)
    {

    }

    public async Task SaveAreaAs()
    {
        var filepath = await SelectSaveFilepathForArea.Handle(Unit.Default);

        if (string.IsNullOrEmpty(filepath))
            return;

        AreaSerializer.Save(filepath, Area);
    }

    public async Task LoadArea()
    {
        var filepath = await SelectLoadFilepathForArea.Handle(Unit.Default);

        if (string.IsNullOrEmpty(filepath))
            return;

        Area = AreaSerializer.Load(filepath);
    }

    public async Task EditSettings()
    {
        await PromptEditSettings.Handle(Unit.Default);
    }

    public void ExportK1()
    {
        Export(GameEngine.K1);
    }
    public void ExportK2()
    {
        Export(GameEngine.K2);
    }
    public void Export(GameEngine game)
    {
        var gamePath = App.ServiceProvider.GetService<AreaDesignerSettingsRoot>()!.Common.Installations.List.First(x => x.Game == game).Path;
        var modPath = Path.Combine(gamePath, @"modules\", "test.mod");

        var mdl = AreaExporter.RoomToMDL(Area.Rooms.First());
        var wok = mdl.GetWalkmesh().GenerateBWM();
        (var mdlData, var mdxData) = MDL.ToBytes(mdl, game, Platform.Windows);

        var ifo = new IFO();
        ifo.ModAreaList.Add("test");
        ifo.EntryArea = "test";
        ifo.Source.Root.SetUInt16("Expansion_Pack", 0);
        ifo.Source.Root.SetList("Mod_GVar_List");
        ifo.Source.Root.SetList("Mod_Expan_List");
        ifo.Source.Root.SetList("Mod_CutSceneList");
        ifo.Source.Root.SetInt32("Mod_Creator_ID", 2);
        ifo.Source.Root.SetUInt32("Mod_Version", 3);
        ifo.Source.Root.SetLocalisedString("Mod_Description", new(-1));
        ifo.Source.Root.SetUInt8("Mod_DawnHour", 0);
        ifo.Source.Root.SetUInt8("Mod_DuskHour", 0);
        ifo.Source.Root.SetUInt8("Mod_IsSaveGame", 0);
        ifo.Source.Root.SetUInt8("Mod_MinPerHour", 1);
        ifo.Source.Root.SetUInt32("Mod_StartYear", 0);
        ifo.Source.Root.SetUInt8("Mod_StartDay", 1);
        ifo.Source.Root.SetUInt8("Mod_StartHour", 13);
        ifo.Source.Root.SetUInt8("Mod_StartMonth", 6);
        ifo.Source.Root.SetString("Mod_Hak", "");
        ifo.Source.Root.SetString("Mod_Tag", "MODULE");
        ifo.Source.Root.SetResRef("Mod_StartMovie", "");
        ifo.Source.Root.SetResRef("Mod_OnSpawnBtnDn", "");
        ifo.Source.Root.SetResRef("Mod_OnPlrRest", "");
        ifo.Source.Root.SetBinary("Mod_ID", Enumerable.Range(0, 16).Select(_ => (byte)0).ToArray());

        var are = new ARE();

        var git = new GFF();
        git.Type = GFFType.GIT;

        var lyt = new LYT();
        lyt.Rooms.Add("test01", 0, 0, 0);

        var erf = new ERF(ERFType.MOD);
        erf.Add("module", ResourceType.IFO, IFO.ToBytes(ifo));
        erf.Add("test", ResourceType.ARE, ARE.ToBytes(are));
        erf.Add("test", ResourceType.GIT, GFF.ToBytes(git));
        erf.Add("test", ResourceType.LYT, LYT.ToBytes(lyt));
        erf.Add("test01", ResourceType.MDL, mdlData);
        erf.Add("test01", ResourceType.MDX, mdxData);
        erf.Add("test01", ResourceType.WOK, BWM.ToBytes(wok));
        ERF.ToFile(erf, modPath);
    }

    public async Task NewArea()
    {
        Area = new();
    }

    public void DeleteSelected()
    {
        foreach (var piece in SelectedPieces)
        {
            DeletePiece(piece);
        }
    }
    public void DeletePiece(object piece)
    {
        // TODO
        //if (piece is UltimateWorldObject @object)
        //{
        //    //@object.Parent.Objects.Remove(@object);
        //}
        //else if (piece)
        //{
        //    //tile.Parent.DeleteTile(tile);
        //}
    }

    public async Task RenderIntercept(OrbitCamera camera, Point mouse, List<IDrawCallDescriptor> descriptors)
    {
        if (Mode is null)
            return;

        await Mode.RenderIntercept(camera, mouse, descriptors);

        if (ActiveWorldObject?.Type == WorldObjectType.Tile)
        {
            descriptors
                .Where(x => ActiveWorldObject.AttachedObjects.Contains(x.Tag))
                .OfType<MeshDescriptor>()
                .ToList()
                .ForEach(x => x.AmbientColor += new Vector3(0.5f, 0.5f, 0.5f));
        }

        descriptors
            .Where(x => ActiveWorldObject != null && x.Tag == ActiveWorldObject)
            .OfType<MeshDescriptor>()
            .ToList()
            .ForEach(x => x.AmbientColor += new Vector3(0.5f, 0.5f, 0.5f));
    }
}
