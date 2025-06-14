using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldResRefPanel : EditFieldPanel<ResRefGFFNodeViewModel, ResRefViewModel, ResRefEditedEventArgs>
{
    public required ResRefPanelViewModel ViewModel
    {
        get => (DataContext as ResRefPanelViewModel)!;
        set => DataContext = value;
    }


    public FieldResRefPanel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new ResRefEditedEventArgs(FinishedEditingEvent, this, ViewModel.SourcePath, ViewModel.Value);
        RaiseEvent(args);
    }

    protected override ResRefViewModel GetCurrentValue()
    {
        return SourceNode?.FieldValue.Clone() ?? new();
    }

    private void TextBox_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseFinishedEditing();
    }
}
