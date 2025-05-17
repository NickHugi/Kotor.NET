using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldDoublePanel : UserControl
{
    public DoubleGFFTreeNodeViewModel Context => (DoubleGFFTreeNodeViewModel)DataContext!;

    public event EventHandler<DoubleEditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<DoubleEditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldDoublePanel, DoubleEditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private Double _originalValue;
    private DoubleGFFTreeNodeViewModel? _contextTransition = default!;

    public FieldDoublePanel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {
            RoutedEventArgs args = new DoubleEditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.FieldValue, _originalValue);
            RaiseEvent(args);
        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.FieldValue;
        }
    }
}
