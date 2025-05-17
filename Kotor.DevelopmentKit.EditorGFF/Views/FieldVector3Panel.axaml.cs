using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldVector3Panel : UserControl
{
    public Vector3GFFTreeNodeViewModel Context => (Vector3GFFTreeNodeViewModel)DataContext!;

    public event EventHandler<Vector3EditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<Vector3EditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldVector3Panel, Vector3EditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private Vector3ViewModel _originalValue;
    private Vector3GFFTreeNodeViewModel? _contextTransition;

    public FieldVector3Panel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {
            RoutedEventArgs args = new Vector3EditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.FieldValue.Clone(), _originalValue.Clone());
            RaiseEvent(args);
        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.FieldValue.Clone();
        }
    }
}
