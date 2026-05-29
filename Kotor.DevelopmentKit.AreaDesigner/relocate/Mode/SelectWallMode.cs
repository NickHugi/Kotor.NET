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

public class SelectWallMode : BaseMode
{
    public required Interaction<Unit, Point> GetMousePoint { get; init; }
    public required Interaction<Wall, Unit> SelectWall { get; init; }

    public override string Name => "Select Wall";

    private Wall? _wall;

    public SelectWallMode(GLEngine engine, Area area, Kit kit, object selectedPiece) : base(engine, area, kit, selectedPiece)
    {
    }

    public override async Task RenderIntercept(OrbitCamera camera, Point mouse, List<MeshDescriptor> descriptors)
    {
        _wall = NearestWallMagnest(camera, mouse.X, mouse.Y)?.Result;

        if (_wall is not null)
            descriptors.Where(x => x.Tag == _wall).ToList().ForEach(x => x.AmbientColor = new(1.5f, 1.5f, 1.5f));
    }

    public override async Task Trigger()
    {
        var template = await SelectWall.Handle(_wall);
    }
}
