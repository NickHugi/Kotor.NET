using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class LocalizedStringEditedEventArgs : RoutedEventArgs
{
    public LocalizedStringGFFTreeNodeViewModel ViewModel { get; }
    public LocalizedStringViewModel NewValue { get; }
    public LocalizedStringViewModel OldValue { get; }

    public LocalizedStringEditedEventArgs(RoutedEvent routedEvent, object source, LocalizedStringGFFTreeNodeViewModel viewModel, LocalizedStringViewModel newValue, LocalizedStringViewModel oldValue)
        : base(routedEvent, source)
    {
        ViewModel = viewModel;
        NewValue = newValue;
        OldValue = oldValue;
    }
}
