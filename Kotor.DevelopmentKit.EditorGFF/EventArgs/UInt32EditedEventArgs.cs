using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class UInt32EditedEventArgs : RoutedEventArgs
{
    public UInt32 NewValue { get; }

    public UInt32EditedEventArgs(RoutedEvent routedEvent, object source, UInt32 newValue)
        : base(routedEvent, source)
    {
        NewValue = newValue;
    }
}
