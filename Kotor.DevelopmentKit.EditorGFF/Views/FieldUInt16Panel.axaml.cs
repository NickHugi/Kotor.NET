using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldUInt16Panel : UserControl
{
    public UInt16GFFTreeNodeViewModel Context => (UInt16GFFTreeNodeViewModel)DataContext!;

    public event EventHandler<UInt16EditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<UInt16EditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldUInt16Panel, UInt16EditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private UInt16 _originalValue;
    private UInt16GFFTreeNodeViewModel? _contextTransition = default!;

    public FieldUInt16Panel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {
            RoutedEventArgs args = new UInt16EditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.FieldValue, _originalValue);
            RaiseEvent(args);
        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.FieldValue;
        }
    }
}
