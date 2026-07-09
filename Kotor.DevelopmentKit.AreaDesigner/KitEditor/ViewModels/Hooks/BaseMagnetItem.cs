using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Magnets;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Hooks;

public abstract class BaseMagnetItem : ReactiveObject
{
    public abstract string Name { get; }
    public abstract MagnetType MagnetType { get; }

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

    public BaseMagnetItem()
    {
        Position = new();
        Orientation = new();
    }

    public abstract UltimateMagnetTemplate ToModel();
}
