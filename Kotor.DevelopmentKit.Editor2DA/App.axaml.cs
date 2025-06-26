using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Kotor.DevelopmentKit.Base;
using Kotor.DevelopmentKit.Base.Extensions;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.Editor2DA;
using Kotor.DevelopmentKit.Editor2DA.ViewModels;
using Kotor.DevelopmentKit.Editor2DA.Views;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Editor2DA;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = default!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

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

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new TwoDAResourceEditor()
            {
                DataContext = new TwoDAResourceEditorViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddBaseServices();

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
        AvaloniaScheduler.Instance.Schedule(() =>
        {
            var context = new ExceptionDialogViewModel()
            {
                Exception = ex,
                Message = ex.Message,
            };
            var dialog = new ExceptionDialog()
            {
                DataContext = context
            };

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                dialog.ShowDialog(desktop.MainWindow!);
            }
        });
    }
}
