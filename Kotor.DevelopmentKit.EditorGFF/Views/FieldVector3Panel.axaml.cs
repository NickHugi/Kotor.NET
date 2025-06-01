using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldVector3Panel : EditFieldPanel<FieldVector3GFFNodeViewModel, Vector3ViewModel, Vector3EditedEventArgs>
{
    public FieldVector3Panel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new Vector3EditedEventArgs(FinishedEditingEvent, this, _transitoryNode, CurrentValue, _transitoryNode.FieldValue);
        RaiseEvent(args);
    }

    protected override Vector3ViewModel GetCurrentValue()
    {
        return SourceNode?.FieldValue.Clone() ?? new();
    }

    private void NumericUpDown_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseFinishedEditing();
    }
}
