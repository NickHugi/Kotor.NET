using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldInt64Panel : EditFieldPanel<Int64GFFNode, Int64, Int64EditedEventArgs>
{
    public required Int64PanelViewModel ViewModel
    {
        get => (DataContext as Int64PanelViewModel)!;
        set => DataContext = value;
    }

    public FieldInt64Panel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new Int64EditedEventArgs(FinishedEditingEvent, this, ViewModel.Value);
        RaiseEvent(args);
    }

    protected override Int64 GetCurrentValue()
    {
        return SourceNode?.FieldValue ?? 0;
    }

    private void NumericUpDown_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseFinishedEditing();
    }
}
