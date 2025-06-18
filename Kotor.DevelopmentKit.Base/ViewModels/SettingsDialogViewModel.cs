using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings;
using Kotor.DevelopmentKit.Base.Settings.Pages;
using Kotor.DevelopmentKit.Base.Settings.Types;
using Kotor.NET.Common;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class SettingsDialogViewModel : ReactiveObject
{
    public IReadOnlyCollection<SettingsPageViewModel> Pages { get; }

    public SettingsPageViewModel? SelectedPage
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public SettingsDialogViewModel()
    {
        var common = new CommonSettings();

        Pages =
            [
                common.GetType().GetCustomAttribute<PageAttribute>().GetViewModel(common)
            ];
    }
}
