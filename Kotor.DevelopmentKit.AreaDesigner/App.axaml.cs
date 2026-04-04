using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.AreaDesigner.ViewModels;
using Kotor.DevelopmentKit.AreaDesigner.Views;

namespace Kotor.DevelopmentKit.AreaDesigner
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new AreaDesignerWindow
                {
                    DataContext = new AreaDesignerViewModel()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

    }
}
