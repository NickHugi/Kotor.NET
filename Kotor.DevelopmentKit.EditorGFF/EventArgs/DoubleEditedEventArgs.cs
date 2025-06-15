using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class DoubleEditedEventArgs : RoutedEventArgs
{
    public Double NewValue { get; }

    public DoubleEditedEventArgs(RoutedEvent routedEvent, object source, Double newValue)
        : base(routedEvent, source)
    {
        NewValue = newValue;
    }
}
