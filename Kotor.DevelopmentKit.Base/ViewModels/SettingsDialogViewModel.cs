using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings;
using Kotor.DevelopmentKit.Base.Settings.Pages;
using Kotor.DevelopmentKit.Base.Settings.Types;
using Kotor.NET.Common;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class SettingsDialogViewModel : ReactiveObject
{
    public DefaultSettingsRoot Settings { get; } = new();
    public IReadOnlyCollection<SettingsPageViewModel> Pages
    {
        get
        {
            return typeof(DefaultSettingsRoot)
                .GetProperties()
                .Where(x => x.PropertyType.GetCustomAttribute<PageAttribute>() is not null)
                .Select(x => x.PropertyType.GetCustomAttribute<PageAttribute>()!.GetViewModel(x))
                .ToList();
        }
    }

    public SettingsPageViewModel? SelectedPage
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public SettingsDialogViewModel()
    {
        var common = new CommonSettings();
    }
}
