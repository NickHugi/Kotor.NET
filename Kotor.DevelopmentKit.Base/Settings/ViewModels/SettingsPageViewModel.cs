using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Types;

namespace Kotor.DevelopmentKit.Base.Settings.ViewModels;

public class SettingsPageViewModel
{
    public string Name { get; }
    public virtual IReadOnlyCollection<SettingsPageViewModel> Pages { get; }
    public SettingsViewModel[] Properties { get; }

    public SettingsPageViewModel(string name, IReadOnlyCollection<SettingsPageViewModel> pages, SettingsViewModel[] properties)
    {
        Name = name;
        Pages = pages;
        Properties = properties;
    }
}
