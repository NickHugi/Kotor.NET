using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.DevelopmentKit.AreaDesigner.ViewModels;
using Kotor.DevelopmentKit.AreaDesigner.Views;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.Renderers.Descriptors;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;

public class ExtendRoomMode : BaseMode
{
    public required Interaction<Unit, Point> GetMousePoint { get; init; }
    public required Interaction<Unit, TileTemplate?> SelectTileTemplate { get; init; }

    public override string Name => "Extend Room";

    private Wall? _wall;
    private bool validWall => _wall is not null && _wall.DoorFrame is null;

    public ExtendRoomMode(GLEngine engine, Area area, ObservableCollection<KitItem> kits, UltimateWorldObject selectedPiece, DesignerSettings settings) : base(engine, area, kits, selectedPiece, settings)
    {
    }

    public override async Task RenderIntercept(OrbitCamera camera, Point mouse, List<IDrawCallDescriptor> descriptors)
    {
        _wall = NearestWallMagnest(camera, (int)mouse.X, (int)mouse.Y)?.Result;

        if (_wall is not null)
        {
            if (!validWall)
                descriptors.Where(x => x.Tag == _wall).OfType<MeshDescriptor>().ToList().ForEach(x => x.AmbientColor = new(1.5f, 0.5f, 0.5f));
            else
                descriptors.Where(x => x.Tag == _wall).OfType<MeshDescriptor>().ToList().ForEach(x => x.AmbientColor = new(1.5f, 1.5f, 1.5f));
        }
    }
}
