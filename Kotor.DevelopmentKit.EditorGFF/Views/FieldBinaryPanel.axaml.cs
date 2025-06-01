using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldBinaryPanel : EditFieldPanel<FieldBinaryGFFNodeViewModel, byte[], BinaryEditedEventArgs>
{
    public FieldBinaryPanel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new BinaryEditedEventArgs(FinishedEditingEvent, this, _transitoryNode, CurrentValue, _transitoryNode.FieldValue);
        RaiseEvent(args);
    }

    protected override byte[] GetCurrentValue()
    {
        return SourceNode?.FieldValue ?? [];
    }
}
