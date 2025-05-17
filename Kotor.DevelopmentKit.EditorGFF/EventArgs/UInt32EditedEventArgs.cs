using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class UInt32EditedEventArgs : RoutedEventArgs
{
    public UInt32GFFTreeNodeViewModel ViewModel { get; }
    public UInt32 NewValue { get; }
    public UInt32 OldValue { get; }

    public UInt32EditedEventArgs(RoutedEvent routedEvent, object source, UInt32GFFTreeNodeViewModel viewModel, UInt32 newValue, UInt32 oldValue)
        : base(routedEvent, source)
    {
        ViewModel = viewModel;
        NewValue = newValue;
        OldValue = oldValue;
    }
}
