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

public class ResRefEditedEventArgs : RoutedEventArgs
{
    public ResRefViewModel NewValue { get; }

    public ResRefEditedEventArgs(RoutedEvent routedEvent, object source, ResRefViewModel newValue)
        : base(routedEvent, source)
    {
        NewValue = newValue.Clone();
    }
}
