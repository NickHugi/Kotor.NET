using System.Collections.ObjectModel;
using System.Linq;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class TileTabViewModel : ReactiveObject
{
    public ObservableCollection<TileItem> TileItems { get; }
    public TileItem? SelectedTileItem
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public object? SelectedTileChildItem
    {
        get;
        set
        {
            this.RaiseAndSetIfChanged(ref field, null);
            this.RaiseAndSetIfChanged(ref field, value);
        }
    }

    public TileTabViewModel()
    {
        TileItems = [];
    }
    public TileTabViewModel(Kit kit) : this()
    {
        TileItems = new ObservableCollection<TileItem>(kit.Tiles.Select(x => new TileItem(x)));
    }


}
