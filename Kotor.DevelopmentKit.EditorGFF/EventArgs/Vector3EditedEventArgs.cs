using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.Models;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class Vector3EditedEventArgs : RoutedEventArgs
{
    public Vector3ViewModel NewValue { get; }

    public Vector3EditedEventArgs(RoutedEvent routedEvent, object source, Vector3ViewModel newValue)
        : base(routedEvent, source)
    {
        NewValue = newValue.Clone();
    }
}
