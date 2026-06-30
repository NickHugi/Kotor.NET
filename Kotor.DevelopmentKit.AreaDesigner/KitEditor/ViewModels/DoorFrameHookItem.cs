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

public class DoorFrameHookItem : HookItem
{
    public override string Name => $"Hook ({Position.X:F2}, {Position.Y:F2}, {Position.Z:F2})";
    public override MagnetType MagnetType => MagnetType.Doorframe;

    public DoorFrameHookItem() : base()
    {
        this.WhenAnyValue(x => x.Position).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
    }
    public DoorFrameHookItem(DoorFrameHookTemplate template) : this()
    {
        Position = new(template.LocalPosition);
        Orientation = new(template.LocalOrientation);
    }

    public override DoorFrameHookTemplate ToModel()
    {
        return new DoorFrameHookTemplate
        {
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
        };
    }
}
