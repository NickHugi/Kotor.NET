using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class Int16EditedEventArgs : RoutedEventArgs
{
    public FieldInt16GFFNodeViewModel EditedNode { get; }
    public Int16 NewValue { get; }
    public Int16 OldValue { get; }

    public Int16EditedEventArgs(RoutedEvent routedEvent, object source, FieldInt16GFFNodeViewModel viewModel, Int16 newValue, Int16 oldValue)
        : base(routedEvent, source)
    {
        EditedNode = viewModel;
        NewValue = newValue;
        OldValue = oldValue;
    }
}
