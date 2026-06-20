using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

public class DesignerSettings : ReactiveObject
{
    public bool PositionSnapEnabled
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public int PositionSnapSize
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
}
