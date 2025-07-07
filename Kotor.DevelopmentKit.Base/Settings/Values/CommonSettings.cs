using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Attributes;

namespace Kotor.DevelopmentKit.Base.Settings.Values;

[Page("Common")]
public class CommonSettings : BaseSettings
{
    [DataMember]
    public InstallationsSettings Installations { get; set; } = new();
}
