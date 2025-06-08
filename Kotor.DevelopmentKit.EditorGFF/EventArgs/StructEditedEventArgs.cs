using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class StructEditedEventArgs : RoutedEventArgs
{
    public IStructGFFTreeNodeViewModel EditedNode { get; }
    public Int32 NewValue { get; }
    public Int32 OldValue { get; }

    public StructEditedEventArgs(RoutedEvent routedEvent, object source, IStructGFFTreeNodeViewModel viewModel, Int32 newValue, Int32 oldValue)
        : base(routedEvent, source)
    {
        EditedNode = viewModel;
        NewValue = newValue;
        OldValue = oldValue;
    }
}
