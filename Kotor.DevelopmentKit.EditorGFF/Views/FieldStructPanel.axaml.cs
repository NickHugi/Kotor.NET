using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldStructPanel : UserControl
{
    public BaseStructGFFTreeNodeViewModel Context => (BaseStructGFFTreeNodeViewModel)DataContext!;

    public event EventHandler<StructEditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<StructEditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldStructPanel, StructEditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private Int32 _originalValue;
    private BaseStructGFFTreeNodeViewModel? _contextTransition = default!;

    public FieldStructPanel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {
            RoutedEventArgs args = new StructEditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.StructID, _originalValue);
            RaiseEvent(args);
        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.StructID;
        }
    }
}
