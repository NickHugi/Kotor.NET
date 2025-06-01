using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldSinglePanel : EditFieldPanel<FieldSingleGFFNodeViewModel, Single, SingleEditedEventArgs>
{
    public FieldSinglePanel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new SingleEditedEventArgs(FinishedEditingEvent, this, _transitoryNode, CurrentValue, _transitoryNode.FieldValue);
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
