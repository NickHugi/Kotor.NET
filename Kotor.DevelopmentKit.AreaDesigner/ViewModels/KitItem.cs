using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.ViewModels;

public class KitItem : ReactiveObject
{
    public bool Active
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public Kit Kit { get; }

    public KitItem(Kit kit)
    {
        Active = false;
        Kit = kit;
    }
}
