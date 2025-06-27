using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldSinglePanel : EditFieldPanel<SingleGFFNode, Single, SingleEditedEventArgs>
{
    public required SinglePanelViewModel ViewModel
    {
        get => (DataContext as SinglePanelViewModel)!;
        set => DataContext = value;
    }


    public FieldSinglePanel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new SingleEditedEventArgs(FinishedEditingEvent, this, ViewModel.Value);
        RaiseEvent(args);
    }

    protected override Single GetCurrentValue()
    {
        return SourceNode?.FieldValue ?? 0;
    }

    private void NumericUpDown_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseFinishedEditing();
    }
}
