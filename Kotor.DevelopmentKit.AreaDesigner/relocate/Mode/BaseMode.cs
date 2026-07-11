using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using DynamicData;
using DynamicData.Binding;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.ViewModels;
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
    public ObservableCollection<KitItem> Kits { get; }

    public WorldObject SelectedWorldObject
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    protected readonly GLEngine _engine;
    protected readonly Area _area;
    protected readonly DesignerSettings _settings;

    protected IReadOnlyCollection<WorldObjectTemplate> _objects => Kits.Where(x => x.Active).SelectMany(x => x.Kit.Objects).ToList();
    protected AreaEntity _areaEntity => _engine.Scene.Entities.OfType<AreaEntity>().Single(x => x.Area == _area);

    public BaseMode(GLEngine engine, Area area, ObservableCollection<KitItem> kits, WorldObject activeWorldObject, DesignerSettings settings)
    {
        Kits = kits;
        SelectedWorldObject = activeWorldObject;
        _engine = engine;
        _area = area;
        _settings = settings;

        Kits.ToObservableChangeSet().AutoRefresh(x => x.Active).Subscribe(_ => this.RaisePropertyChanged(nameof(_objects)));
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

    protected IEnumerable<RaycastResult<WorldObject>> RaycastWorldObject(OrbitCamera camera, double x, double y)
    {
        var ray = camera.ProjectRay((int)x, (int)y, _engine.Width, _engine.Height);

        return _area.Rooms
            .SelectMany(x => x.AllObjects)
            .OrderBy(x => ray.ShortestDistanceTo(x.GlobalPosition))
            .Select(x => new RaycastResult<WorldObject>(x, ray.ShortestDistanceTo(x.GlobalPosition)))
            .Where(x => x.Distance < 3)
            .ToList();
    }

    protected IEnumerable<MagnetResult<Magnet>> NearbyMagnets(ICollection<Magnet> candidates, float distance)
    {
        var near = new List<MagnetResult<Magnet>>();
        var allMagnets = _area.Rooms.SelectMany(x => x.AllMagnets);

        foreach (var magnet in candidates)
        {
            var match = allMagnets
                .Where(x => Vector3.Distance(magnet.GlobalPosition, x.GlobalPosition) < distance)
                .OrderBy(x => Vector3.Distance(magnet.GlobalPosition, x.GlobalPosition))
                .Select(x => new MagnetResult<Magnet>(magnet, x, Vector3.Distance(magnet.GlobalPosition, x.GlobalPosition)))
                .ToList();

            if (match.Count > 0)
                near.AddRange(match);
        }

        return near.OrderBy(x => x.Distance);
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
