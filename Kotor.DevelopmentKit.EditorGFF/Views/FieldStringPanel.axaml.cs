using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldStringPanel : UserControl
{
    public StringGFFTreeNodeViewModel Context => (StringGFFTreeNodeViewModel)DataContext!;

    public event EventHandler<StringEditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<StringEditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldStringPanel, StringEditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private String _originalValue;
    private StringGFFTreeNodeViewModel? _contextTransition = default!;

    public FieldStringPanel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {
            RoutedEventArgs args = new StringEditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.FieldValue, _originalValue);
            RaiseEvent(args);
        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.FieldValue;
        }
    }
}
