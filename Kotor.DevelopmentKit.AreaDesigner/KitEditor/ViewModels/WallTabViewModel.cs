using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class WallTabViewModel : ReactiveObject
{
    public ObservableCollection<WallItem> WallItems { get; }
    public WallItem? SelectedWallItem
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public WallTabViewModel()
    {
        WallItems = [];
    }
    public WallTabViewModel(Kit kit) : this()
    {
        WallItems = new ObservableCollection<WallItem>(kit.Walls.Select(x => new WallItem(x)));
    }

    public void AddWall()
    {
        WallItems.Add(new());
    }

    public void DeleteSelectedWall()
    {
        if (SelectedWallItem is null)
            return;

        WallItems.Remove(SelectedWallItem);
    }
}
