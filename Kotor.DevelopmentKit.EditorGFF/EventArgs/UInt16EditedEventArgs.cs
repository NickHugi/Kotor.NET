using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class UInt16EditedEventArgs : RoutedEventArgs
{
    public UInt16GFFTreeNodeViewModel ViewModel { get; }
    public UInt16 NewValue { get; }
    public UInt16 OldValue { get; }

    public UInt16EditedEventArgs(RoutedEvent routedEvent, object source, UInt16GFFTreeNodeViewModel viewModel, UInt16 newValue, UInt16 oldValue)
        : base(routedEvent, source)
    {
        ViewModel = viewModel;
        NewValue = newValue;
        OldValue = oldValue;
    }
}
