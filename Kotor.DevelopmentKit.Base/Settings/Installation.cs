using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Types.String;
using Kotor.NET.Common;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.Settings;

public class Installation : ReactiveObject
{
    [StringSetting("Name", "The name that will be used by the toolset to reference this installation")]
    public required string Name
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    [StringSetting("Path", "The path to the root directory of the KotOR installlation.")]
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
