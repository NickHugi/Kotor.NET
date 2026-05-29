using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
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

public class BaseMode : ReactiveObject
{
    public virtual string Name { get; } = "";

    public Kit? SelectedKit { get; set; }
    public object SelectedPiece { get; set; }

    protected readonly GLEngine _engine;
    protected readonly Area _area;

    protected AreaEntity _areaEntity => _engine.Scene.Entities.OfType<AreaEntity>().Single(x => x.Area == _area);

    public BaseMode(GLEngine engine, Area area, Kit selectedKit, object selectedPiece)
    {
        SelectedKit = selectedKit;
        SelectedPiece = selectedPiece;
        _engine = engine;
        _area = area;
    }

    public virtual Task RenderIntercept(OrbitCamera camera, Point mouse, List<MeshDescriptor> descriptors)
    {
        return Task.CompletedTask;
    }

    public virtual Task Trigger()
    {
        return Task.CompletedTask;
    }

    public virtual Task AlternativeTrigger()
    {
        return Task.CompletedTask;
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
    
    protected MagnetResult<Wall>? NearestAdjacentWall(Room room, float distance)
    {
        var near = new List<MagnetResult<Wall>>();
        var otherWalls = _area.Rooms.SelectMany(x => x.Walls).ToList();

        foreach (var wall in room.Walls)
        {
            var match = otherWalls
                .Where(x => x.Template.Group == wall.Template.Group)
                .Where(x => Vector3.Distance(wall.Position, x.Position) < distance)
                .OrderBy(x => Vector3.Distance(wall.Position, x.Position))
                .Select(x => new MagnetResult<Wall>(wall, x, Vector3.Distance(wall.Position, x.Position)))
                .ToList();

            if (match.Count > 0)
                near.AddRange(match);
        }

        return near.OrderBy(x => x.Distance).FirstOrDefault();
    }
}

public class MagnetResult<T>
{
    public T Source { get; }
    public T Target { get; }
    public float Distance { get; }

    public MagnetResult(T source, T target, float distance)
    {
        Source = source;
        Target = target;
        Distance = distance;
    }
}
