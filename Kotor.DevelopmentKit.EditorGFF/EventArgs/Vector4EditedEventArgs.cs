using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class Vector4EditedEventArgs : RoutedEventArgs
{
    public FieldVector4GFFNodeViewModel EditedNode { get; }
    public Vector4ViewModel NewValue { get; }
    public Vector4ViewModel OldValue { get; }

    public Vector4EditedEventArgs(RoutedEvent routedEvent, object source, FieldVector4GFFNodeViewModel viewModel, Vector4ViewModel newValue, Vector4ViewModel oldValue)
        : base(routedEvent, source)
    {
        EditedNode = viewModel;
        NewValue = newValue.Clone();
        OldValue = oldValue.Clone();
    }
}
