using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldUInt32Panel : EditFieldPanel<UInt32GFFNode, UInt32, UInt32EditedEventArgs>
{
    public required UInt32PanelViewModel ViewModel
    {
        get => (DataContext as UInt32PanelViewModel)!;
        set => DataContext = value;
    }


    public FieldUInt32Panel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new UInt32EditedEventArgs(FinishedEditingEvent, this, ViewModel.Value);
        RaiseEvent(args);
    }

    protected override UInt32 GetCurrentValue()
    {
        return SourceNode?.FieldValue ?? 0;
    }

    private void NumericUpDown_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseFinishedEditing();
    }
}
