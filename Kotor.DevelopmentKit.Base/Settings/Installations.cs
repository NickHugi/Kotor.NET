using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Pages;
using Kotor.DevelopmentKit.Base.Settings.Types.Boolean;
using Kotor.DevelopmentKit.Base.Settings.Types.Table;
using Kotor.NET.Common;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.Settings;

[Page("Installation")]
public class Installations : ReactiveObject
{
    [DataMember]
    [BooleanSetting(
        "Automatically Check for Installations",
        @"When enabled, the toolset on startup will automatically check for existing KotOR
          installations that have not yet been to the list."
    )]
    public bool AutoCheckForInstallations
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    [DataMember]
    [TableSetting(
        "KotOR Installations",
        "Configure the list of different KotOR installations that the toolset is aware of."
    )]
    public ObservableCollection<Installation> List { get; } = [];
}
