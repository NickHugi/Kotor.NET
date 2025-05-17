using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldResRefPanel : UserControl
{
    public ResRefGFFTreeNodeViewModel Context => (ResRefGFFTreeNodeViewModel)DataContext!;

    public event EventHandler<ResRefEditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<ResRefEditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldResRefPanel, ResRefEditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private ResRefViewModel _originalValue;
    private ResRefGFFTreeNodeViewModel? _contextTransition;

    public FieldResRefPanel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {
            RoutedEventArgs args = new ResRefEditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.FieldValue.Clone(), _originalValue.Clone());
            RaiseEvent(args);
        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.FieldValue;
        }
    }
}
