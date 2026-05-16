using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaExportation;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaSerialization;
using Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;
using Kotor.DevelopmentKit.AreaDesigner.Settings;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Graphics.OpenGL;
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
    public Interaction<Unit, WallTemplate?> SelectWallTemplate = new();
    public Interaction<Unit, TileTemplate?> SelectTileTemplate = new();
    public Interaction<Unit, string?> SelectSaveFilepathForArea = new();
    public Interaction<Unit, string?> SelectLoadFilepathForArea = new();
    public Interaction<Unit, Unit> PromptEditSettings = new();

    public ObservableCollection<Kit> Kits { get; } = new();
    public Kit? SelectedKit
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public AreaDesignerViewModel()
    {
        Kit.Manager.Refresh();
        Kits = new(Kit.Manager.Kits);
    }

    public Area Area
    {
        get => Engine.Scene.Entities.OfType<AreaEntity>().Single().Area;
        set
        {
            Engine.Scene.Entities.OfType<AreaEntity>().Single().Area = value;
            this.RaisePropertyChanged(nameof(Area));
        }
    }

    public GLEngine Engine { get; set => this.RaiseAndSetIfChanged(ref field, value); }

    public BaseMode Mode { get; private set => this.RaiseAndSetIfChanged(ref field, value); }

    public bool ShowWalls
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = true;

    public bool ShowDoors
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = true; 

    public bool ShowCorners
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = true;

    public bool ShowCeilings
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = false;

    public void SetSceneMode_AddRoom()
    {
        var area = Engine.Scene.Entities.OfType<AreaEntity>().Single().Area;
        Mode = new AddRoomMode(Engine, area)
        {

        };
    }
    public void SetSceneMode_DeleteRoom()
    {

    }
    public void SetSceneMode_AddTile()
    {
        var area = Engine.Scene.Entities.OfType<AreaEntity>().Single().Area;
        Mode = new ExtendRoomMode(Engine, area)
        {
            GetMousePoint = GetMousePoint,
            SelectTileTemplate = SelectTileTemplate,
        };
    }
    public void SetSceneMode_DeleteTile()
    {

    }
    public void SetSceneMode_SwitchWall()
    {
        var area = Engine.Scene.Entities.OfType<AreaEntity>().Single().Area;
        Mode = new SwitchWallMode(Engine, area)
        {
            GetMousePoint = GetMousePoint,
            SelectWallTemplate = SelectWallTemplate,
        };
    }
    public void SetSceneMode_AddObject()
    {
        var area = Engine.Scene.Entities.OfType<AreaEntity>().Single().Area;
        Mode = new AddObjectMode(Engine, area);
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
}
