using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class Int8EditedEventArgs : RoutedEventArgs
{
    public FieldInt8GFFNodeViewModel EditedNode { get; }
    public sbyte NewValue { get; }
    public sbyte OldValue { get; }

    public Int8EditedEventArgs(RoutedEvent routedEvent, object source, FieldInt8GFFNodeViewModel viewModel, sbyte newValue, sbyte oldValue)
        : base(routedEvent, source)
    {
        EditedNode = viewModel;
        NewValue = newValue;
        OldValue = oldValue;
    }
}
