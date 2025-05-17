using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;
using Kotor.NET.Common.Data;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldLocalizedStringPanel : UserControl
{
    public LocalizedStringGFFTreeNodeViewModel Context => (LocalizedStringGFFTreeNodeViewModel)DataContext!;

    public event EventHandler<LocalizedStringEditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<LocalizedStringEditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<FieldLocalizedStringPanel, LocalizedStringEditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    private LocalizedStringViewModel _originalValue;
    private LocalizedStringGFFTreeNodeViewModel? _contextTransition;

    public FieldLocalizedStringPanel()
    {
        InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        if (Context is null)
        {

        }
        else
        {
            _contextTransition = Context;
            _originalValue = Context.FieldValue.Clone();
        }
    }

    public void AddSubString()
    {
        Context.FieldValue.AddSubString();
        EmitEvent();
    }
    public void RemoveSelectedSubString()
    {
        Context.FieldValue.RemoveSelectedSubString();
        EmitEvent();
    }

    private void EmitEvent()
    {
        RoutedEventArgs args = new LocalizedStringEditedEventArgs(FinishedEditingEvent, this, _contextTransition, _contextTransition.FieldValue.Clone(), _originalValue.Clone());
        RaiseEvent(args);
        _originalValue = Context.FieldValue.Clone();
    }
}
