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

public class HookItem : ReactiveObject
{
    public virtual string Name => "Hook";
    public virtual MagnetType MagnetType => MagnetType.Magnet;
    public ReactiveVector3 Position
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public ReactiveQuaternion Orientation
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public HookItem()
    {
        Position = new();
        Orientation = new();
    }

    public virtual HookTemplate ToModel()
    {
        return new HookTemplate
        {
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
        };
    }
}
