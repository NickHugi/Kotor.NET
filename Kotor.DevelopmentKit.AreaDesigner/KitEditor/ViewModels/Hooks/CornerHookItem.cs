using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Markup.Xaml.Templates;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Hooks;

public class CornerHookItem : BaseMagnetItem
{
    public override string Name => $"{TemplateID} ({Position.Z:F2}, {Position.Y:F2}, {Position.Z:F2})";
    public override MagnetType MagnetType => MagnetType.Hook;

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

    public CornerHookItem() : base()
    {
        KitID = "";
        TemplateID = "";

        this.WhenAnyValue(x => x.TemplateID).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
        this.WhenAnyValue(x => x.Position).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
    }
    public CornerHookItem(UltimateMagnetTemplate template) : this()
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;
        Position = new(template.LocalPosition);
        Orientation = new(template.LocalOrientation);
    }

    public override UltimateMagnetTemplate ToModel()
    {
        return new UltimateMagnetTemplate
        {
            KitID = KitID,
            TemplateID = TemplateID,
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
            MagnetType = MagnetType.Hook,
        };
    }
}
