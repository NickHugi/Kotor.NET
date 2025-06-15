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

public class Vector4EditedEventArgs : RoutedEventArgs
{
    public Vector4ViewModel NewValue { get; }

    public Vector4EditedEventArgs(RoutedEvent routedEvent, object source, Vector4ViewModel newValue)
        : base(routedEvent, source)
    {
        NewValue = newValue.Clone();
    }
}
