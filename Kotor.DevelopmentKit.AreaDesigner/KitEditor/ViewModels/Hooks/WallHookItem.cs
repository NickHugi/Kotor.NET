using System;
using System.Collections.ObjectModel;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Hooks;

public class WallHookItem : BaseMagnetItem
{
    public override string Name => $"{TemplateID} ({Position.X:F2}, {Position.Y:F2}, {Position.Z:F2})";
    public override MagnetType MagnetType => MagnetType.Wall;

    public string KitID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string TemplateID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public ObservableCollection<int> AdjacentWalls
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public WallHookItem() : base()
    {
        KitID = "";
        TemplateID = "";
        AdjacentWalls = [];

        this.WhenAnyValue(x => x.TemplateID).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
        this.WhenAnyValue(x => x.Position).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
    }
    public WallHookItem(WallHookTemplate wallHook) : this()
    {
        KitID = wallHook.KitID;
        TemplateID = wallHook.TemplateID;
        Position = new(wallHook.LocalPosition);
        Orientation = new(wallHook.LocalOrientation);
        AdjacentWalls = new(wallHook.AdjacentWalls);
    }

    public override WallHookTemplate ToModel()
    {
        return new WallHookTemplate
        {
            KitID = KitID,
            TemplateID = TemplateID,
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
        };
    }
}
