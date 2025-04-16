using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Kotor.DevelopmentKit.Base;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.Editor2DA;
using Kotor.DevelopmentKit.Editor2DA.ViewModels;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Editor2DA
{
    public partial class App : Application
    {
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
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new TwoDAResourceEditor()
                {
                    DataContext = new TwoDAResourceEditorViewModel()
                };
            }

            base.OnFrameworkInitializationCompleted();
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
}
