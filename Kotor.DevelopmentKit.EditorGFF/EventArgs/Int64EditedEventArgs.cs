using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class Int64EditedEventArgs : RoutedEventArgs
{
    public Int64GFFTreeNodeViewModel ViewModel { get; }
    public Int64 NewValue { get; }
    public Int64 OldValue { get; }

    public Int64EditedEventArgs(RoutedEvent routedEvent, object source, Int64GFFTreeNodeViewModel viewModel, Int64 newValue, Int64 oldValue)
        : base(routedEvent, source)
    {
        ViewModel = viewModel;
        NewValue = newValue;
        OldValue = oldValue;
    }
}
