using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.Settings;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Settings;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Views;
using Kotor.NET.Interfaces;
using Kotor.NET.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Kotor.DevelopmentKit.EditorGFF;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = default!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        ServiceProvider = BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var viewModel = ServiceProvider.GetService<GFFResourceEditorViewModel>();
            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddSingleton<DefaultSettingsRoot, GFFSettingsRoot>();
        services.AddScoped<SettingsDialogViewModel>();
        services.AddScoped<GFFResourceEditorViewModel>();
        services.AddScoped<ITalkTableLookup, TalkTableLookup>();

        return services.BuildServiceProvider();
    }
}
