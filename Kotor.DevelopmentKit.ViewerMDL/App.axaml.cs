using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.ViewerMDL.ViewModels;
using Kotor.DevelopmentKit.ViewerMDL.Views;

namespace Kotor.DevelopmentKit.ViewerMDL;

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
            desktop.MainWindow = new Views.MDLResourceViewer
            {
                DataContext = new ViewModels.MDLResourceViewerViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

}
