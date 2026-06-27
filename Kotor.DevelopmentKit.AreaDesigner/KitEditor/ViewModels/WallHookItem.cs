using System;
using System.Collections.ObjectModel;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class WallHookItem : HookItem
{
    public override string Name => $"{DefaultTemplateID} ({Position.X:F2}, {Position.Y:F2}, {Position.Z:F2})";
    public string DefaultTemplateID
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
        DefaultTemplateID = "";
        AdjacentWalls = [];

        this.WhenAnyValue(x => x.DefaultTemplateID).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
        this.WhenAnyValue(x => x.Position).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
    }
    public WallHookItem(WallHookTemplate wallHook) : this()
    {
        DefaultTemplateID = wallHook.DefaultTemplateID;
        Position = new(wallHook.LocalPosition);
        Orientation = new(wallHook.LocalOrientation);
        AdjacentWalls = new(wallHook.AdjacentWalls);
    }

    public WallHookTemplate ToModel()
    {
        return new WallHookTemplate
        {
            DefaultTemplateID = DefaultTemplateID,
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
        };
    }
}
