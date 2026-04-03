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

    public KitEditorViewModel()
    {
        TileTab = new();
    }
    public KitEditorViewModel(Kit kit) : this()
    {
        TileTab = new(kit);
    }

}
