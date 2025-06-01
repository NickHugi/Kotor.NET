using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldStructPanel : EditFieldPanel<IStructGFFTreeNodeViewModel, Int32, StructEditedEventArgs>
{
    public FieldStructPanel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new StructEditedEventArgs(FinishedEditingEvent, this, _transitoryNode, CurrentValue, _transitoryNode.StructID);
        RaiseEvent(args);
    }

    protected override Int32 GetCurrentValue()
    {
        return SourceNode?.StructID ?? 0;
    }

    private void NumericUpDown_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseFinishedEditing();
    }
}
