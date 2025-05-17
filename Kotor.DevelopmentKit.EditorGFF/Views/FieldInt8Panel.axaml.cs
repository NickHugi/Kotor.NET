using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldInt8Panel : UserControl
{
    public Int8GFFTreeNodeViewModel Context => (Int8GFFTreeNodeViewModel)DataContext!;

    public event EventHandler<Int8EditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<Int8EditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldInt8Panel, Int8EditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private SByte _originalValue;
    private Int8GFFTreeNodeViewModel? _contextTransition = default!;

    public FieldInt8Panel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {
            RoutedEventArgs args = new Int8EditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.FieldValue, _originalValue);
            RaiseEvent(args);
        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.FieldValue;
        }
    }
}
