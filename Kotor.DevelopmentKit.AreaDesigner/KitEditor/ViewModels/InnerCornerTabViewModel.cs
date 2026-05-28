using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class InnerCornerTabViewModel : ReactiveObject
{
    public ObservableCollection<InnerCornerItem> InnerCornerItems { get; }
    public InnerCornerItem? SelectedInnerCornerItem
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public InnerCornerTabViewModel()
    {
        InnerCornerItems = [];
    }
    public InnerCornerTabViewModel(Kit kit) : this()
    {
        InnerCornerItems = new ObservableCollection<InnerCornerItem>(kit.InnerCorners.Select(x => new InnerCornerItem(x)));
    }

    public void AddInnerCorner()
    {
        InnerCornerItems.Add(new());
    }

    public void DeleteSelectedInnerCorner()
    {
        if (SelectedInnerCornerItem is null)
            return;

        InnerCornerItems.Remove(SelectedInnerCornerItem);
    }
}
