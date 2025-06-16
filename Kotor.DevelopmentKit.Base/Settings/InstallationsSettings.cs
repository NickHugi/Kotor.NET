using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;

namespace Kotor.DevelopmentKit.Base.Settings;

public class InstallationsSettings : BaseSettingPage
{
    public override string PageName => "Installations";

    public ObservableCollection<Installation> Installations { get; } =
    [
        new Installation()
        {
            Name = "KotOR",
            Path = "",
            Game = GameEngine.K1,
            Platform = Platform.Windows,
        }
    ];
}
