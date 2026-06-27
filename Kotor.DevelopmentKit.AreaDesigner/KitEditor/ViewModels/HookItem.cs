using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public abstract class HookItem : ReactiveObject
{
    public virtual string Name => "Hook";
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
}
