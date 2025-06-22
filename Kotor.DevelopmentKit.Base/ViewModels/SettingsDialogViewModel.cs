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
    public IReadOnlyCollection<SettingsPageViewModel> Pages
    {
        get
        {
            return _settings
                .GetType()
                .GetProperties()
                .Where(x => x.PropertyType.GetCustomAttribute<PageAttribute>() is not null)
                .Select(x => x.PropertyType.GetCustomAttribute<PageAttribute>()!.GetViewModel(x.GetValue(_settings)))
                .ToList();
        }
    }

    public SettingsPageViewModel? SelectedPage
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    private readonly DefaultSettingsRoot _settings;


    public SettingsDialogViewModel(DefaultSettingsRoot settings)
    {
        _settings = settings;
    }
}
