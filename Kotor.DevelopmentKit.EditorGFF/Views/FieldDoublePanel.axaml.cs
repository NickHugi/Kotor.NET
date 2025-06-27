using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldDoublePanel : EditFieldPanel<DoubleGFFNode, Double, DoubleEditedEventArgs>
{
    public required DoublePanelViewModel ViewModel
    {
        get => (DataContext as DoublePanelViewModel)!;
        set => DataContext = value;
    }


    public FieldDoublePanel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new DoubleEditedEventArgs(FinishedEditingEvent, this, ViewModel.Value);
        RaiseEvent(args);
    }

    protected override Double GetCurrentValue()
    {
        return SourceNode?.FieldValue ?? 0;
    }


    private void NumericUpDown_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseFinishedEditing();
    }
}
