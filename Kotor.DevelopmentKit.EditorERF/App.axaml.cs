using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorERF.ViewModels;
using Kotor.DevelopmentKit.EditorERF.Views;

namespace Kotor.DevelopmentKit.EditorERF
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
                desktop.MainWindow = new ERFResourceEditor
                {
                    DataContext = new ERFResourceEditorViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

    }
}
