using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class Vector3EditedEventArgs : RoutedEventArgs
{
    public Vector3GFFTreeNodeViewModel ViewModel { get; }
    public Vector3ViewModel NewValue { get; }
    public Vector3ViewModel OldValue { get; }

    public Vector3EditedEventArgs(RoutedEvent routedEvent, object source, Vector3GFFTreeNodeViewModel viewModel, Vector3ViewModel newValue, Vector3ViewModel oldValue)
        : base(routedEvent, source)
    {
        ViewModel = viewModel;
        NewValue = newValue.Clone();
        OldValue = oldValue.Clone();
    }
}
