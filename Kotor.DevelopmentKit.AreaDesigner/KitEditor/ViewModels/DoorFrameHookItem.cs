using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class DoorFrameHookItem : ReactiveObject
{
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

    public DoorFrameHookItem()
    {
        Position = new();
        Orientation = new();
    }
    public DoorFrameHookItem(DoorFrameHookTemplate template)
    {
        Position = new(template.Position);
        Orientation = new(template.Orientation);
    }
}
