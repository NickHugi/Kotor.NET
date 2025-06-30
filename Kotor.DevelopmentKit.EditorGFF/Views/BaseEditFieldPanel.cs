using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;

namespace Kotor.DevelopmentKit.EditorGFF.Views;


public abstract class BaseEditFieldPanel : UserControl
{

}


public abstract class EditFieldPanel<TNodeViewModel, TValueViewModel, TEventArgs>
    : BaseEditFieldPanel
    where TNodeViewModel : class
    where TEventArgs : RoutedEventArgs
{
    public event EventHandler<UInt16EditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<TEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<BaseEditFieldPanel, TEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    public GFFViewModel GFF
    {
        get => GetValue(GFFProperty);
        set => SetValue(GFFProperty, value);
    }
    public static readonly StyledProperty<GFFViewModel> GFFProperty =
        AvaloniaProperty.Register<BaseEditFieldPanel, GFFViewModel>(nameof(GFF));

    public TNodeViewModel SourceNode
    {
        get => GetValue(SourceNodeProperty);
        set => SetValue(SourceNodeProperty, value);
    }
    public static readonly StyledProperty<TNodeViewModel> SourceNodeProperty =
        AvaloniaProperty.Register<BaseEditFieldPanel, TNodeViewModel>(nameof(SourceNode));

    public TValueViewModel CurrentValue
    {
        get => GetValue(CurrentValueProperty);
        set => SetValue(CurrentValueProperty, value);
    }
    public static readonly StyledProperty<TValueViewModel> CurrentValueProperty =
        AvaloniaProperty.Register<BaseEditFieldPanel, TValueViewModel>(nameof(CurrentValue));

    protected TNodeViewModel? _transitoryNode;

    public EditFieldPanel()
    {
        this.GetObservable(SourceNodeProperty).Subscribe(newNode =>
        {
            if (_transitoryNode is not null)
            {
                RaiseFinishedEditing();
            }

            CurrentValue = GetCurrentValue();
            _transitoryNode = newNode;
        });
    }

    protected abstract void RaiseFinishedEditing();

    protected abstract TValueViewModel GetCurrentValue();
}
