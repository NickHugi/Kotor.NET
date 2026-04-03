using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class FloorTabViewModel : ReactiveObject
{
    public ObservableCollection<FloorItem> FloorItems { get; }
    public FloorItem? SelectedFloorItem
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public FloorTabViewModel()
    {
        FloorItems = [];
    }
    public FloorTabViewModel(Kit kit) : this()
    {
        FloorItems = new ObservableCollection<FloorItem>(kit.Floors.Select(x => new FloorItem(x)));
    }

    public void AddFloor()
    {
        FloorItems.Add(new());
    }

    public void DeleteSelectedFloor()
    {
        if (SelectedFloorItem is null)
            return;

        FloorItems.Remove(SelectedFloorItem);
    }
}
