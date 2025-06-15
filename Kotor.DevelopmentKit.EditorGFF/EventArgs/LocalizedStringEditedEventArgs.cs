using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class LocalizedStringEditedEventArgs : RoutedEventArgs
{
    public LocalizedStringViewModel NewValue { get; }

    public LocalizedStringEditedEventArgs(RoutedEvent routedEvent, object source, LocalizedStringViewModel newValue)
        : base(routedEvent, source)
    {
        NewValue = newValue.Clone();
    }
}
