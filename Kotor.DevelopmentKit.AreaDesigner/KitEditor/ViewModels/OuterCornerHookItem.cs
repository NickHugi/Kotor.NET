using System.Collections.ObjectModel;
using System.Linq;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class OuterCornerHookItem : ReactiveObject
{
    public string Name => $"Hook ({Position.Z:F2}, {Position.Y:F2}, {Position.Z:F2})";

    public string DefaultCornerID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ReactiveVector3 Position
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ReactiveQuaternion Orientation
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ObservableCollection<int> AdjacentWalls
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public OuterCornerHookItem()
    {
        DefaultCornerID = "";
        Position = new();
        Orientation = new();
        AdjacentWalls = [];
    }
    public OuterCornerHookItem(OuterCornerHookTemplate template)
    {
        DefaultCornerID = template.DefaultCornerID;
        Position = new(template.LocalPosition);
        Orientation = new(template.LocalOrientation);
        AdjacentWalls = new(template.Adjacent);
    }

    public OuterCornerHookTemplate ToModel()
    {
        return new OuterCornerHookTemplate
        {
            DefaultCornerID = DefaultCornerID,
            Adjacent = AdjacentWalls.ToArray(),
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
        };
    }
}
