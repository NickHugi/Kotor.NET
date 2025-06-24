using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.Settings.Values;

public class DefaultSettingsRoot : ReactiveObject
{
    public const string SettingsFilepath = "settings.json";

    [DataMember]
    public CommonSettings Common { get; set; } = new();
}
