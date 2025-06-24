using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Attributes;

namespace Kotor.DevelopmentKit.Base.Settings;

[Page("Common")]
public class CommonSettings
{
    public Installations Installations { get; set; } = new();
}
