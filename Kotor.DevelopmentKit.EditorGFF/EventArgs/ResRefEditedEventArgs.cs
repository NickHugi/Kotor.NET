using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.Models;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class ResRefEditedEventArgs : RoutedEventArgs
{
    public ReactiveResRef NewValue { get; }

    public ResRefEditedEventArgs(RoutedEvent routedEvent, object source, ReactiveResRef newValue)
        : base(routedEvent, source)
    {
        NewValue = newValue.Clone();
    }
}
