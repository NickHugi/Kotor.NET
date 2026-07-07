using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Markup.Xaml.Templates;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Hooks;

public class CornerHookItem : BaseMagnetItem
{
    public override string Name => $"{InnerTemplateID} ({Position.Z:F2}, {Position.Y:F2}, {Position.Z:F2})";
    public override MagnetType MagnetType => MagnetType.Hook;

    public string InnerKitID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string InnerTemplateID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string OuterKitID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string OuterTemplateID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ObservableCollection<int> AdjacentWalls
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public CornerHookItem() : base()
    {
        InnerKitID = "";
        InnerTemplateID = "";
        OuterKitID = "";
        OuterTemplateID = "";

        AdjacentWalls = [];

        this.WhenAnyValue(x => x.InnerTemplateID).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
        this.WhenAnyValue(x => x.OuterTemplateID).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
        this.WhenAnyValue(x => x.Position).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
    }
    public CornerHookItem(CornerHookTemplate template) : this()
    {
        InnerKitID = template.InnerKitID;
        InnerTemplateID = template.InnerTemplateID;
        OuterKitID = template.OuterKitID;
        OuterTemplateID = template.OuterTemplateID;
        Position = new(template.LocalPosition);
        Orientation = new(template.LocalOrientation);
    }

    public override CornerHookTemplate ToModel()
    {
        return new CornerHookTemplate
        {
            InnerKitID = InnerKitID,
            InnerTemplateID = InnerTemplateID,
            OuterKitID = OuterKitID,
            OuterTemplateID = OuterTemplateID,
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
        };
    }
}
