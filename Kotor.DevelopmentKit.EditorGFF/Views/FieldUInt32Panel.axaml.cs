using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldUInt32Panel : UserControl
{
    public UInt32GFFTreeNodeViewModel Context => (UInt32GFFTreeNodeViewModel)DataContext!;

    public event EventHandler<UInt32EditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<UInt32EditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldUInt32Panel, UInt32EditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private UInt32 _originalValue;
    private UInt32GFFTreeNodeViewModel? _contextTransition = default!;

    public FieldUInt32Panel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {
            RoutedEventArgs args = new UInt32EditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.FieldValue, _originalValue);
            RaiseEvent(args);
        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.FieldValue;
        }
    }
}
