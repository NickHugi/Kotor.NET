using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldVector4Panel : UserControl
{
    public Vector4GFFTreeNodeViewModel Context => (Vector4GFFTreeNodeViewModel)DataContext!;

    public event EventHandler<Vector4EditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<Vector4EditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldVector4Panel, Vector4EditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private Vector4ViewModel _originalValue;
    private Vector4GFFTreeNodeViewModel? _contextTransition;

    public FieldVector4Panel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {
            RoutedEventArgs args = new Vector4EditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.FieldValue.Clone(), _originalValue.Clone());
            RaiseEvent(args);
        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.FieldValue;
        }
    }
}
