using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class DoubleEditedEventArgs : RoutedEventArgs
{
    public FieldDoubleGFFNodeViewModel ViewModel { get; }
    public Double NewValue { get; }
    public Double OldValue { get; }

    public DoubleEditedEventArgs(RoutedEvent routedEvent, object source, FieldDoubleGFFNodeViewModel viewModel, Double newValue, Double oldValue)
        : base(routedEvent, source)
    {
        ViewModel = viewModel;
        NewValue = newValue;
        OldValue = oldValue;
    }
}
