using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Kotor.DevelopmentKit.Base.ViewModels;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.Extensions;

public static class ApplicationExtensions
{
    public static void HandleExceptions(this Application self)
    {
        AppDomain.CurrentDomain.UnhandledException += self.OnUnhandledException;
        TaskScheduler.UnobservedTaskException += self.OnUnobservedTaskException;
        RxApp.DefaultExceptionHandler = Observer.Create<Exception>(self.HandleException);
    }

    private static void OnUnhandledException(this Application self, object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            self.HandleException(ex);
        }
    }
    private static void OnUnobservedTaskException(this Application self, object sender, UnobservedTaskExceptionEventArgs e)
    {
        if (e.Exception.InnerException is not null)
        {
            self.HandleException(e.Exception.InnerException);
            e.SetObserved(); // Prevents application crash
        }
    }

    private static void HandleException(this Application self, Exception ex)
    {
        AvaloniaScheduler.Instance.Schedule(Unit.Default, (_, _) =>
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

            if (self.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                dialog.ShowDialog(desktop.MainWindow!);
            }

            return Disposable.Empty;
        });
    }
}
