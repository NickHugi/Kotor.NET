using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.Models;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class UInt8EditedEventArgs : RoutedEventArgs
{
    public byte NewValue { get; }

    public UInt8EditedEventArgs(RoutedEvent routedEvent, object source, byte newValue)
        : base(routedEvent, source)
    {
        NewValue = newValue;
    }
}
