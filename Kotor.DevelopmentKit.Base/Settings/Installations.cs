using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Pages;
using Kotor.DevelopmentKit.Base.Settings.Types.Boolean;
using Kotor.DevelopmentKit.Base.Settings.Types.Table;

namespace Kotor.DevelopmentKit.Base.Settings;

[Page("Installation")]
public class Installations
{
    [BooleanSetting(
        "Automatically Check for Installations",
        "When enabled, the toolset will automatically check for existing KotOR installations that have not yet been to the list."
    )]
    public bool AutoCheckForInstallations { get; set; }

    [TableSetting(
        "KotOR Installations",
        "Configure the list of different KotOR installations that the toolset is aware of."
    )]
    public ICollection<Installation> List { get; set; } =
        [
            new Installation() { Name = "K1", Path = @"C:\K1", Game = NET.Common.GameEngine.K1, Platform = NET.Common.Platform.Windows }
        ];

}
