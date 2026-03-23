using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Kotor.DevelopmentKit.AreaDesigner.Views;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.OpenGL;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;

public class BaseMode
{
    public virtual string Name { get; }

    protected readonly GLEngine _engine;
    protected readonly Area _area;

    protected AreaEntity _areaEntity => _engine.Scene.Entities.OfType<AreaEntity>().Single(x => x.Area == _area);

    public BaseMode(GLEngine engine, Area area)
    {
        _engine = engine;
        _area = area;
    }

    public virtual async Task RenderIntercept(OrbitCamera camera, Point mouse, List<MeshDescriptor> descriptors)
    {
    }

    public virtual async Task Trigger()
    {
    }

    protected RaycastResult<Wall>? NearestWallMagnest(OrbitCamera camera, double x, double y)
    {
        var ray = camera.ProjectRay((int)x, (int)y, _engine.Width, _engine.Height);

        return _area.Rooms
            .SelectMany(x => x.Walls)
            .Where(x => x.LinkedTile is null)
            .OrderBy(x => ray.ShortestDistanceTo(x.Position))
            .Select(x => new RaycastResult<Wall>(x, ray.ShortestDistanceTo(x.Position)))
            .Where(x => x.Distance < 3)
            .FirstOrDefault();
    }

    protected (Wall ThisHook, Wall OtherHook, float distance) NearestAdjacentWall(Room room)
    {
        var near = new List<(Wall NewHook, Wall OldHook, float distance)>();
        var otherWalls = _area.Rooms.SelectMany(x => x.Walls).ToList();

        foreach (var wall in room.Walls)
        {
            var match = otherWalls
                .Where(x => x.DoorFrame is not null)
                .Where(x => Vector3.Distance(wall.Position, x.Position) < 3)
                .OrderBy(x => Vector3.Distance(wall.Position, x.Position))
                .Select(x => (wall, x, Vector3.Distance(wall.Position, x.Position)))
                .ToList();

            if (match.Count > 0)
                near.AddRange(match);
        }

        return near.OrderBy(x => x.distance).FirstOrDefault();
    }
}
