using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.Base.Settings;

public class DefaultSettingsRoot
{
    public CommonSettings Common { get; set; } = new();

    private readonly string _path = "./settings.json";

    public DefaultSettingsRoot()
    {
        if (Path.Exists(_path))
        {

        }
        else
        {

        }
    }
}
