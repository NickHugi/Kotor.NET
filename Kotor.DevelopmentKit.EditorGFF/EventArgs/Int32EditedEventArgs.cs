using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class Int32EditedEventArgs : RoutedEventArgs
{
    public Int32GFFTreeNodeViewModel ViewModel { get; }
    public Int32 NewValue { get; }
    public Int32 OldValue { get; }

    public Int32EditedEventArgs(RoutedEvent routedEvent, object source, Int32GFFTreeNodeViewModel viewModel, Int32 newValue, Int32 oldValue)
        : base(routedEvent, source)
    {
        ViewModel = viewModel;
        NewValue = newValue;
        OldValue = oldValue;
    }
}
