using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class SingleEditedEventArgs : RoutedEventArgs
{
    public SingleGFFTreeNodeViewModel ViewModel { get; }
    public Single NewValue { get; }
    public Single OldValue { get; }

    public SingleEditedEventArgs(RoutedEvent routedEvent, object source, SingleGFFTreeNodeViewModel viewModel, Single newValue, Single oldValue)
        : base(routedEvent, source)
    {
        ViewModel = viewModel;
        NewValue = newValue;
        OldValue = oldValue;
    }
}
