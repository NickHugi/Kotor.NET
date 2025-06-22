using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.Base.Settings;

public class DefaultSettingsRoot
{
    public CommonSettings Common { get; set; } = new();
}
