using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class ResRefEditedEventArgs : RoutedEventArgs
{
    public FieldResRefGFFNodeViewModel ViewModel { get; }
    public ResRefViewModel NewValue { get; }
    public ResRefViewModel OldValue { get; }

    public ResRefEditedEventArgs(RoutedEvent routedEvent, object source, FieldResRefGFFNodeViewModel viewModel, ResRefViewModel newValue, ResRefViewModel oldValue)
        : base(routedEvent, source)
    {
        ViewModel = viewModel;
        NewValue = newValue.Clone();
        OldValue = oldValue.Clone();
    }
}
