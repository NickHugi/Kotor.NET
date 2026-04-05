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

public class SwitchWallMode : BaseMode
{
    public required Interaction<Unit, Point> GetMousePoint { get; init; }
    public required Interaction<Unit, WallTemplate> SelectWallTemplate { get; init; }

    public override string Name => "Switch Wall";

    private Wall? _wall;

    public SwitchWallMode(GLEngine engine, Area area) : base(engine, area)
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
        var template = await SelectWallTemplate.Handle(Unit.Default);

        if (_wall is not null && template is not null)
        {
            _wall.KitID = template.KitID;
            _wall.TemplateID = template.ID;
        }
    }
}
