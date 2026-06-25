using System.Collections.ObjectModel;
using System.Linq;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class OuterCornerHookItem : HookItem
{
    public string Name => $"{DefaultTemplateID} ({Position.Z:F2}, {Position.Y:F2}, {Position.Z:F2})";

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

    public OuterCornerHookItem() : base()
    {
        DefaultTemplateID = "";
        AdjacentWalls = [];

        //this.WhenAnyValue(x => x.DefaultTemplateID).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
        //this.WhenAnyValue(x => x.Position).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
    }
    public OuterCornerHookItem(OuterCornerHookTemplate template) : this()
    {
        DefaultTemplateID = template.DefaultTemplateID;
        Position = new(template.LocalPosition);
        Orientation = new(template.LocalOrientation);
        AdjacentWalls = new(template.Adjacent);
    }

    public OuterCornerHookTemplate ToModel()
    {
        return new OuterCornerHookTemplate
        {
            DefaultTemplateID = DefaultTemplateID,
            Adjacent = AdjacentWalls.ToArray(),
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
        };
    }
}
