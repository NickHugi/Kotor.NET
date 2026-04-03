using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class KitEditorViewModel : ReactiveObject
{
    public TileTabViewModel TileTab { get; }
    public FloorTabViewModel FloorTab { get; }
    public WallTabViewModel WallTab { get; }
    public DoorFrameTabViewModel DoorFrameTab { get; }
    public CeilingTabViewModel CeilingTab { get; }
    public ObjectTabViewModel ObjectTab { get; }

    public KitEditorViewModel()
    {
        TileTab = new();
        FloorTab = new();
        WallTab = new();
        DoorFrameTab = new();
        CeilingTab = new();
        ObjectTab = new();
    }
    public KitEditorViewModel(Kit kit) : this()
    {
        TileTab = new(kit);
        FloorTab = new(kit);
        WallTab = new(kit);
        DoorFrameTab = new(kit);
        CeilingTab = new(kit);
        ObjectTab = new(kit);
    }

}
