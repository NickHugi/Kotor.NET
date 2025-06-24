using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.Base.Settings;

public class DefaultSettingsRoot
{
    public const string SettingsFilepath = "settings.json";

    public CommonSettings Common { get; set; } = new();

    public DefaultSettingsRoot()
    {
    }
}
