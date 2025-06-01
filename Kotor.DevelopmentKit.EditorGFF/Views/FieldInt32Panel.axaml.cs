using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldInt32Panel : EditFieldPanel<FieldInt32GFFNodeViewModel, Int32, Int32EditedEventArgs>
{
    public FieldInt32Panel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new Int32EditedEventArgs(FinishedEditingEvent, this, _transitoryNode, CurrentValue, _transitoryNode.FieldValue);
        RaiseEvent(args);
    }

    protected override Int32 GetCurrentValue()
    {
        return SourceNode?.FieldValue ?? 0;
    }

    private void NumericUpDown_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseFinishedEditing();
    }
}
