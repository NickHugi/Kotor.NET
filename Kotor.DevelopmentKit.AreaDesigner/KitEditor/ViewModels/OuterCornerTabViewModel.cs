using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class OuterCornerTabViewModel : ReactiveObject
{
    public ObservableCollection<OuterCornerItem> OuterCornerItems { get; }
    public OuterCornerItem? SelectedOuterCornerItem
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public OuterCornerTabViewModel()
    {
        OuterCornerItems = [];
    }
    public OuterCornerTabViewModel(Kit kit) : this()
    {
        OuterCornerItems = new ObservableCollection<OuterCornerItem>(kit.OuterCorners.Select(x => new OuterCornerItem(x)));
    }

    public void AddOuterCorner()
    {
        OuterCornerItems.Add(new());
    }

    public void DeleteSelectedOuterCorner()
    {
        if (SelectedOuterCornerItem is null)
            return;

        OuterCornerItems.Remove(SelectedOuterCornerItem);
    }
}
