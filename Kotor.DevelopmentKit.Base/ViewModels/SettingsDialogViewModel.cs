using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings;
using Kotor.NET.Common;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class SettingsDialogViewModel : ReactiveObject
{
    public IReadOnlyCollection<BaseSettingPage> Settings { get; } =
    [
        new StubPage("Common",
            [
                new InstallationsSettings()
            ]
        ),
    ];

    public BaseSettingPage SelectedPage
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
}
