using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class UInt8EditedEventArgs : RoutedEventArgs
{
    public UInt8GFFTreeNodeViewModel ViewModel { get; }
    public byte NewValue { get; }
    public byte OldValue { get; }

    public UInt8EditedEventArgs(RoutedEvent routedEvent, object source, UInt8GFFTreeNodeViewModel viewModel, byte newValue, byte oldValue)
        : base(routedEvent, source)
    {
        ViewModel = viewModel;
        NewValue = newValue;
        OldValue = oldValue;
    }
}
