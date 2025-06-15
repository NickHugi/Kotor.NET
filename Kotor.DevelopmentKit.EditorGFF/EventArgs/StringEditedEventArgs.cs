using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class StringEditedEventArgs : RoutedEventArgs
{
    public String NewValue { get; }

    public StringEditedEventArgs(RoutedEvent routedEvent, object source, String newValue)
        : base(routedEvent, source)
    {
        NewValue = newValue;
    }
}
