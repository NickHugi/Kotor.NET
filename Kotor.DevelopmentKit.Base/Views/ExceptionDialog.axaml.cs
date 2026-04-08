using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.ViewModels;

namespace Kotor.DevelopmentKit.Base;

public partial class ExceptionDialog : Window
{
    public ExceptionDialogViewModel Context => (ExceptionDialogViewModel)DataContext!;

    public ExceptionDialog()
    {
        InitializeComponent();
    }

    public void Ok()
    {
        Close();
    }

    public async Task Copy()
    {
        if (Clipboard is not null)
        {
            await Clipboard.SetTextAsync(Context.StackTrace);
        }
    }

    public static async Task ShowDilaog(Window window, Exception exception)
    {
        var dialog = new ExceptionDialog()
        {
            DataContext = new ExceptionDialogViewModel { Exception = exception, Message = exception.Message }
        };
        await dialog.ShowDialog(window);
    }
}
