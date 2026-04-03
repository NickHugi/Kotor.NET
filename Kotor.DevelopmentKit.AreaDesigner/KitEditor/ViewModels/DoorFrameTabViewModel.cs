using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class DoorFrameTabViewModel : ReactiveObject
{
    public ObservableCollection<DoorFrameItem> DoorFrameItems { get; }
    public DoorFrameItem? SelectedDoorFrameItem
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public DoorFrameTabViewModel()
    {
        DoorFrameItems = [];
    }
    public DoorFrameTabViewModel(Kit kit) : this()
    {
        DoorFrameItems = new ObservableCollection<DoorFrameItem>(kit.DoorFrames.Select(x => new DoorFrameItem(x)));
    }

    public void AddDoorFrame()
    {
        DoorFrameItems.Add(new());
    }

    public void DeleteSelectedDoorFrame()
    {
        if (SelectedDoorFrameItem is null)
            return;

        DoorFrameItems.Remove(SelectedDoorFrameItem);
    }
}
