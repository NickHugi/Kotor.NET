using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class FloorHookItem : HookItem
{
    public override string Name => $"{TemplateID} ({Position.X:F2}, {Position.Y:F2}, {Position.Z:F2})";
    public override MagnetType MagnetType => MagnetType.Floor;

    public string KitID
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string TemplateID
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public FloorHookItem() : base()
    {
        KitID = "";
        TemplateID = "";

        this.WhenAnyValue(x => x.TemplateID).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
        this.WhenAnyValue(x => x.Position).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
    }
    public FloorHookItem(DoorFrameHookTemplate template) : this()
    {
        Position = new(template.LocalPosition);
        Orientation = new(template.LocalOrientation);
    }

    public override FloorHookTemplate ToModel()
    {
        return new FloorHookTemplate
        {
            KitID = KitID,
            TemplateID = TemplateID,
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
        };
    }
}
