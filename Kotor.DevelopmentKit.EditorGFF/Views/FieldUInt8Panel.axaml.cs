using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DynamicData.Binding;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldUInt8Panel : EditFieldPanel<FieldUInt8GFFNodeViewModel, Byte, UInt8EditedEventArgs>
{
    public FieldUInt8Panel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new UInt8EditedEventArgs(FinishedEditingEvent, this, _transitoryNode, CurrentValue, _transitoryNode.FieldValue);
        RaiseEvent(args);
    }

    protected override Byte GetCurrentValue()
    {
        return SourceNode?.FieldValue ?? 0;
    }

    private void NumericUpDown_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseFinishedEditing();
    }
}
