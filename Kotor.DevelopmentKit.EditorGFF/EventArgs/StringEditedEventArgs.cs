using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class StringEditedEventArgs : RoutedEventArgs
{
    public FieldStringGFFNodeViewModel EditedNode { get; }
    public String NewValue { get; }
    public String OldValue { get; }

    public StringEditedEventArgs(RoutedEvent routedEvent, object source, FieldStringGFFNodeViewModel viewModel, String newValue, String oldValue)
        : base(routedEvent, source)
    {
        EditedNode = viewModel;
        NewValue = newValue;
        OldValue = oldValue;
    }
}
