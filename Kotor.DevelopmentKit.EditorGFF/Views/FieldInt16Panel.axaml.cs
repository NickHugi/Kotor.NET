using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldInt16Panel : UserControl
{
    public Int16GFFTreeNodeViewModel Context => (Int16GFFTreeNodeViewModel)DataContext!;

    public event EventHandler<Int16EditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<Int16EditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldInt16Panel, Int16EditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private Int16 _originalValue;
    private Int16GFFTreeNodeViewModel? _contextTransition = default!;

    public FieldInt16Panel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {
            RoutedEventArgs args = new Int16EditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.FieldValue, _originalValue);
            RaiseEvent(args);
        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.FieldValue;
        }
    }
}
