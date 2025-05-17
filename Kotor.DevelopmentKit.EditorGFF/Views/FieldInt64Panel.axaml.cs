using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldInt64Panel : UserControl
{
    public Int64GFFTreeNodeViewModel Context => (Int64GFFTreeNodeViewModel)DataContext!;

    public event EventHandler<Int64EditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<Int64EditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldInt64Panel, Int64EditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private Int64 _originalValue;
    private Int64GFFTreeNodeViewModel? _contextTransition = default!;

    public FieldInt64Panel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {
            RoutedEventArgs args = new Int64EditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.FieldValue, _originalValue);
            RaiseEvent(args);
        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.FieldValue;
        }
    }
}
