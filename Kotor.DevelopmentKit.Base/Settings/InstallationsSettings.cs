using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Pages;
using Kotor.DevelopmentKit.Base.Settings.Types.Boolean;
using Kotor.DevelopmentKit.Base.Settings.Types.Integer;
using Kotor.DevelopmentKit.Base.Settings.Types.String;
using Kotor.NET.Common;

namespace Kotor.DevelopmentKit.Base.Settings;

[Page("Installation")]
public class InstallationsSettings
{
    [BooleanSetting("Test Boolean", "Very descriptve")]
    public bool TestBoolean { get; set; } = true;

    [StringSetting("Very string", "how much wood would a woodchuck chuck if a woodchuck could chuck wood?")]
    public string TestString { get; set; } = "abc";

    [IntegerSetting("Test Integer", "Integer value 100001")]
    public int TestInt { get; set; } = 123;

    //public ObservableCollection<Installation> Installations { get; } =
    //[
    //    new Installation()
    //    {
    //        Name = "KotOR",
    //        Path = "",
    //        Game = GameEngine.K1,
    //        Platform = Platform.Windows,
    //    }
    //];
}
