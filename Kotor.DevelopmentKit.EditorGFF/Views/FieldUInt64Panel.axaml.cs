using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldUInt64Panel : UserControl
{
    public UInt64GFFTreeNodeViewModel Context => (UInt64GFFTreeNodeViewModel)DataContext!;

    public event EventHandler<UInt64EditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<UInt64EditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldUInt64Panel, UInt64EditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private UInt64 _originalValue;
    private UInt64GFFTreeNodeViewModel? _contextTransition = default!;

    public FieldUInt64Panel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {
            RoutedEventArgs args = new UInt64EditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.FieldValue, _originalValue);
            RaiseEvent(args);
        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.FieldValue;
        }
    }
}
