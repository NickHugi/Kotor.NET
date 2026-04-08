using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Types.Enum;
using Kotor.DevelopmentKit.Base.Settings.Types.String;
using Kotor.NET.Common;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.Settings.Values;

public class InstallationSettings : BaseSettings
{
    [DataMember]
    [StringSetting("Name", "The name that will be used by the toolset to reference this installation")]
    public required string Name
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = "";

    [DataMember]
    [StringSetting("Path", "The path to the root directory of the KotOR installlation.")]
    public required string Path
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = "";

    [DataMember]
    [EnumSetting("Game", "Which KotOR game the installation is linked to.")]
    public required GameEngine Game
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    [DataMember]
    [EnumSetting("Platform", "Which platform the installation is targetting.")]
    public required Platform Platform
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
}
