using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldInt32Panel : UserControl
{
    public Int32GFFTreeNodeViewModel Context => (Int32GFFTreeNodeViewModel)DataContext!;

    public event EventHandler<Int32EditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<Int32EditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldInt32Panel, Int32EditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private Int32 _originalValue;
    private Int32GFFTreeNodeViewModel? _contextTransition = default!;

    public FieldInt32Panel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {
            RoutedEventArgs args = new Int32EditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.FieldValue, _originalValue);
            RaiseEvent(args);
        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.FieldValue;
        }
    }
}
