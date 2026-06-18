using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.Views;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.Renderers.Descriptors;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;

public class BaseMode : ReactiveObject
{
    public virtual string Name { get; } = "";

    public Kit? SelectedKit
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public object SelectedPiece
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

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

    public virtual Task RenderIntercept(OrbitCamera camera, Point mouse, List<IDrawCallDescriptor> descriptors)
    {
        return Task.CompletedTask;
    }

    public virtual Task Update(float delta)
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
    public virtual Task KeyPress(Inputs inputs, int keyCode)
    {
        return Task.CompletedTask;
    }

    protected RaycastResult<Wall>? NearestWallMagnest(OrbitCamera camera, double x, double y)
    {
        var ray = camera.ProjectRay((int)x, (int)y, _engine.Width, _engine.Height);

        return _area.Rooms
            .SelectMany(x => x.Objects)
            .OfType<Tile>()
            .SelectMany(x => x.Walls)
            .OfType<Wall>()
            .Where(x => x.LinkedTile is null)
            .OrderBy(x => ray.ShortestDistanceTo(x.GlobalPosition))
            .Select(x => new RaycastResult<Wall>(x, ray.ShortestDistanceTo(x.GlobalPosition)))
            .Where(x => x.Distance < 3)
            .FirstOrDefault();
    }
    protected RaycastResult<Floor>? IntersectingFloor(OrbitCamera camera, double x, double y)
    {
        var ray = camera.ProjectRay((int)x, (int)y, _engine.Width, _engine.Height);

        return _area.Rooms
            .SelectMany(x => x.Objects)
            .OfType<Floor>()
            .OrderBy(x => ray.ShortestDistanceTo(x.GlobalPosition))
            .Select(x => new RaycastResult<Floor>(x, ray.ShortestDistanceTo(x.GlobalPosition)))
            .Where(x => x.Distance < 3)
            .FirstOrDefault();
    }
    protected RaycastResult<Ceiling>? IntersectingCeiling(OrbitCamera camera, double x, double y)
    {
        var ray = camera.ProjectRay((int)x, (int)y, _engine.Width, _engine.Height);

        return _area.Rooms
            .SelectMany(x => x.Objects)
            .OfType<Ceiling>()
            .OrderBy(x => ray.ShortestDistanceTo(x.GlobalPosition))
            .Select(x => new RaycastResult<Ceiling>(x, ray.ShortestDistanceTo(x.GlobalPosition)))
            .Where(x => x.Distance < 3)
            .FirstOrDefault();
    }
    protected RaycastResult<IWorldObject>? IntersectingObject(OrbitCamera camera, double x, double y)
    {
        var ray = camera.ProjectRay((int)x, (int)y, _engine.Width, _engine.Height);

        return _area.Rooms
            .SelectMany(x => x.Objects)
            .Concat(_area.Rooms.SelectMany(x => x.Objects.OfType<Tile>().SelectMany(x => x.VirtualObjects)))
            .OrderBy(x => ray.ShortestDistanceTo(x.GlobalPosition))
            .Select(x => new RaycastResult<IWorldObject>(x, ray.ShortestDistanceTo(x.GlobalPosition)))
            .Where(x => x.Distance < 1)
            .FirstOrDefault();
    }

    protected MagnetResult<Wall>? NearestAdjacentWall(Room room, float distance)
    {
        var near = new List<MagnetResult<Wall>>();
        var otherWalls = _area.Rooms.SelectMany(x => x.Objects).OfType<Wall>().ToList();

        foreach (var wall in room.Objects.OfType<Wall>())
        {
            var match = otherWalls
                .Where(x => x.Template.ClassID == wall.Template.ClassID)
                .Where(x => Vector3.Distance(wall.GlobalPosition, x.GlobalPosition) < distance)
                .OrderBy(x => Vector3.Distance(wall.GlobalPosition, x.GlobalPosition))
                .Select(x => new MagnetResult<Wall>(wall, x, Vector3.Distance(wall.GlobalPosition, x.GlobalPosition)))
                .ToList();

            if (match.Count > 0)
                near.AddRange(match);
        }

        return near.OrderBy(x => x.Distance).FirstOrDefault();
    }

    protected MagnetResult<Magnet>? NearestMagnet(List<Magnet> magnets, float distance)
    {
        var near = new List<MagnetResult<Magnet>>();
        var otherWalls = _area.Rooms.SelectMany(x => x.GetMagnets());

        foreach (var magnet in magnets)
        {
            var match = otherWalls
                //.Where(x => x.Template.ClassID == wall.Template.ClassID)
                .Where(x => Vector3.Distance(magnet.GlobalPosition, x.GlobalPosition) < distance)
                .OrderBy(x => Vector3.Distance(magnet.GlobalPosition, x.GlobalPosition))
                .Select(x => new MagnetResult<Magnet>(magnet, x, Vector3.Distance(magnet.GlobalPosition, x.GlobalPosition)))
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
