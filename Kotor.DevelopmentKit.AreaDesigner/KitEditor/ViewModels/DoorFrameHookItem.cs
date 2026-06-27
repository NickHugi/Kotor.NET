using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class DoorFrameHookItem : HookItem
{
    public override string Name => $"Hook ({Position.X:F2}, {Position.Y:F2}, {Position.Z:F2})";

    public DoorFrameHookItem() : base()
    {
        this.WhenAnyValue(x => x.Position).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
    }
    public DoorFrameHookItem(DoorFrameHookTemplate template) : this()
    {
        Position = new(template.Position);
        Orientation = new(template.Orientation);
    }

    public DoorFrameHookTemplate ToModel()
    {
        return new DoorFrameHookTemplate
        {
            Position = Position.ToModel(),
            Orientation = Orientation.ToModel(),
        };
    }
}
