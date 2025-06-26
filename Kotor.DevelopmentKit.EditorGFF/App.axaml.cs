using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Kotor.DevelopmentKit.Base;
using Kotor.DevelopmentKit.Base.Extensions;
using Kotor.DevelopmentKit.Base.Settings.Interfaces;
using Kotor.DevelopmentKit.Base.Settings.Services;
using Kotor.DevelopmentKit.Base.Settings.Values;
using Kotor.DevelopmentKit.Base.Settings.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Extensions;
using Kotor.DevelopmentKit.EditorGFF.Settings;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Views;
using Kotor.NET.Helpers;
using Kotor.NET.Interfaces;
using Kotor.NET.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = default!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        // TODO extension
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        RxApp.DefaultExceptionHandler = Observer.Create<Exception>(ex =>
        {
            HandleException(ex);
        });
    }

    public override void OnFrameworkInitializationCompleted()
    {
        ServiceProvider = BuildServiceProvider();

        ServiceProvider.GetService<IInstallationsScanner>()?.CheckAndAdd();

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

        services.AddBaseServices();
        services.AddSettings<GFFSettingsRoot>();
        services.AddGFFEditorServices();        

        return services.BuildServiceProvider();
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            HandleException(ex);
        }
    }
    private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        HandleException(e.Exception.InnerException);
        e.SetObserved(); // Prevents application crash
    }

    private void HandleException(Exception ex)
    {
        //AvaloniaScheduler.Instance.Schedule(() =>
        //{
        //    var context = new ExceptionDialogViewModel()
        //    {
        //        Exception = ex,
        //        Message = ex.Message,
        //    };
        //    var dialog = new ExceptionDialog()
        //    {
        //        DataContext = context
        //    };

        //    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        //    {
        //        dialog.ShowDialog(desktop.MainWindow!);
        //    }
        //});
    }
}
