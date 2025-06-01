using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class BinaryEditedEventArgs : RoutedEventArgs
{
    public FieldBinaryGFFNodeViewModel ViewModel { get; }
    public byte[] NewValue { get; }
    public byte[] OldValue { get; }

    public BinaryEditedEventArgs(RoutedEvent routedEvent, object source, FieldBinaryGFFNodeViewModel viewModel, byte[] newValue, byte[] oldValue)
        : base(routedEvent, source)
    {
        ViewModel = viewModel;
        NewValue = newValue;
        OldValue = oldValue;
    }
}
