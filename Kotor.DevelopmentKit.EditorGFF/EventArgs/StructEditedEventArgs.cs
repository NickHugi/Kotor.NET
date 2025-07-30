using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Models;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class StructEditedEventArgs : RoutedEventArgs
{
    public Int32 NewValue { get; }

    public StructEditedEventArgs(RoutedEvent routedEvent, object source, Int32 newValue)
        : base(routedEvent, source)
    {
        NewValue = newValue;
    }
}
