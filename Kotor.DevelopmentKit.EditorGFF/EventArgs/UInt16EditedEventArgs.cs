using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class UInt16EditedEventArgs : RoutedEventArgs
{
    public UInt16 NewValue { get; }

    public UInt16EditedEventArgs(RoutedEvent routedEvent, object source, UInt16 newValue)
        : base(routedEvent, source)
    {
        NewValue = newValue;
    }
}
