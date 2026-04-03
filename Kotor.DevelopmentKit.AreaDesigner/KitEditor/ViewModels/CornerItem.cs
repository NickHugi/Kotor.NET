using System.Collections.ObjectModel;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class CornerItem : ReactiveObject
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

    public CornerItem()
    {
        DefaultCornerID = "";
        Position = new();
        Orientation = new();
        AdjacentWalls = [];
    }
    public CornerItem(CornerTemplate template)
    {
        DefaultCornerID = template.ID;
        Position = new(template.Position);
        Orientation = new(template.Orientation);
        AdjacentWalls = new(template.Adjacent);
    }
}
