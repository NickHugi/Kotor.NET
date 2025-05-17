using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DynamicData.Binding;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldUInt8Panel : UserControl
{
    public UInt8GFFTreeNodeViewModel Context => (UInt8GFFTreeNodeViewModel)DataContext!;

    public event EventHandler<UInt8EditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<UInt8EditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldUInt8Panel, UInt8EditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private byte _originalValue;
    private UInt8GFFTreeNodeViewModel? _contextTransition = default!;

    public FieldUInt8Panel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {
            RoutedEventArgs args = new UInt8EditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.FieldValue, _originalValue);
            RaiseEvent(args);
        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.FieldValue;
        }
    }
}
