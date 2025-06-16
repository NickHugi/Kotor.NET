using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.Settings;

public class Installation : ReactiveObject
{
    public required string Name
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public required string Path
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public required GameEngine Game
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public required Platform Platform
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
}
