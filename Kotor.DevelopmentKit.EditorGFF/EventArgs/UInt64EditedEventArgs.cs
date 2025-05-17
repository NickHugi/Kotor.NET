using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class UInt64EditedEventArgs : RoutedEventArgs
{
    public UInt64GFFTreeNodeViewModel ViewModel { get; }
    public UInt64 NewValue { get; }
    public UInt64 OldValue { get; }

    public UInt64EditedEventArgs(RoutedEvent routedEvent, object source, UInt64GFFTreeNodeViewModel viewModel, UInt64 newValue, UInt64 oldValue)
        : base(routedEvent, source)
    {
        ViewModel = viewModel;
        NewValue = newValue;
        OldValue = oldValue;
    }
}
