using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.Models;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class LocalizedStringEditedEventArgs : RoutedEventArgs
{
    public ReactiveLocalizedString NewValue { get; }

    public LocalizedStringEditedEventArgs(RoutedEvent routedEvent, object source, ReactiveLocalizedString newValue)
        : base(routedEvent, source)
    {
        NewValue = newValue.Clone();
    }
}
