using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Kotor.DevelopmentKit.AreaDesigner.Views;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.OpenGL;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;

public class ExtendRoomMode : BaseMode
{
    public required Interaction<Unit, Point> GetMousePoint { get; init; }
    public required Interaction<Unit, TileTemplate?> SelectTileTemplate { get; init; }

    public override string Name => "Extend Room";

    private Wall? _wall;
    private bool validWall => _wall is not null && _wall.DoorFrame is null;

    public ExtendRoomMode(GLEngine engine, Area area) : base(engine, area)
    {
    }

    public override async Task RenderIntercept(OrbitCamera camera, Point mouse, List<MeshDescriptor> descriptors)
    {
        _wall = NearestWallMagnest(camera, (int)mouse.X, (int)mouse.Y)?.Result;

        if (_wall is not null)
        {
            if (!validWall)
                descriptors.Where(x => x.Tag == _wall).ToList().ForEach(x => x.AmbientColor = new(1.5f, 0.5f, 0.5f));
            else
                descriptors.Where(x => x.Tag == _wall).ToList().ForEach(x => x.AmbientColor = new(1.5f, 1.5f, 1.5f));
        }
    }

    public override async Task Trigger()
    {
        //if (validWall)
        //    _wall!.Extend(TileTemplate.Sandral8x8);
    }

    public override async Task AlternativeTrigger()
    {
        if (!validWall)
            return;

        var template = await SelectTileTemplate.Handle(Unit.Default);

        if (template is null)
            return;

        var tile = _wall!.Extend(template);
    }
}
