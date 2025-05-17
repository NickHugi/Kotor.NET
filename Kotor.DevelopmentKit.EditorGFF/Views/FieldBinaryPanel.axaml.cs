using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldBinaryPanel : UserControl
{
    public BinaryGFFTreeNodeViewModel Context => (BinaryGFFTreeNodeViewModel)DataContext!;

    public event EventHandler<BinaryEditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<BinaryEditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldBinaryPanel, BinaryEditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private byte[] _originalValue = default!;
    private BinaryGFFTreeNodeViewModel? _contextTransition = default!;

    public FieldBinaryPanel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {
            RoutedEventArgs args = new BinaryEditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.FieldValue, _originalValue);
            RaiseEvent(args);
        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.FieldValue;
        }
    }
}
