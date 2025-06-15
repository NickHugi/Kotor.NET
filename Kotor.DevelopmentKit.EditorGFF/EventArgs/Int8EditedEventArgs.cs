using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class Int8EditedEventArgs : RoutedEventArgs
{
    public sbyte NewValue { get; }

    public Int8EditedEventArgs(RoutedEvent routedEvent, object source, sbyte newValue)
        : base(routedEvent, source)
    {
        NewValue = newValue;
    }
}
