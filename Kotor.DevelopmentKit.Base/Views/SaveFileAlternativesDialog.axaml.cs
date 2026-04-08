using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.DialogResults;
using Kotor.DevelopmentKit.Base.ViewModels;

namespace Kotor.DevelopmentKit.Base;

public partial class SaveFileAlternativesDialog : Window
{
    public required SaveFileAlternativesDialogViewModel Context
    {
        get => (SaveFileAlternativesDialogViewModel)DataContext!;
        init => DataContext = value;
    }

    public SaveFileAlternativesDialog()
    {
        InitializeComponent();
    }

    public void SaveToOverride()
    {
        Close(SaveFileAlternativesDialogResult.ToOverride);
    }

    public void SaveToMOD()
    {
        Close(SaveFileAlternativesDialogResult.ToMOD);
    }

    public void SaveToFile()
    {
        Close(SaveFileAlternativesDialogResult.ToFile);
    }

    public void Cancel()
    {
        Close(SaveFileAlternativesDialogResult.None);
    }
}
