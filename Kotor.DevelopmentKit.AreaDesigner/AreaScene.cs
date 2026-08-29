using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Renderers.Descriptors;
using Silk.NET.Core.Native;

namespace Kotor.DevelopmentKit.AreaDesigner;

public class AreaScene : IScene
{
    private Vector2 _previousMouse;
    public Vector2 MouseDelta { get; private set; }
    public Vector2 Mouse
    {
        get => field;
        set
        {
            _previousMouse = field;
            field = value;
            MouseDelta = Mouse - _previousMouse;
        }
    }

    public float RunningTime { get; set; }

    public Camera ActiveCamera { get; } = new OrbitCamera()
    {
        Distance = 5,
        Pitch = 1,
        Target = new(0, 0, 2),
    };
    public Area Area { get; set; } = new();
    public List<WorldObject> Projection { get; set; } = [];
    public BaseMode? Mode { get; set; }

    public bool DoRenderCorners { get; set; } = true;
    public bool DoRenderWalls { get; set; } = true;
    public bool DoRenderDoors { get; set; } = true;
    public bool DoRenderFloor { get; set; } = true;
    public bool DoRenderCeiling { get; set; } = false;
    public bool DoRenderObjects { get; set; } = true;

    public void Update(IAssetManager assets, float timestep)
    {
        Projection.Clear();
        Area.Invalidate();

        Mode?.Update(timestep, this);
        var inject = Area.Rooms.FirstOrDefault();

        RunningTime += timestep;
    }

    public IEnumerable<IDrawCallDescriptor> Render(IAssetManager assets)
    {
        var descriptors = GetDrawCallDescriptors(assets);
        Mode?.Render(0, this, ref descriptors);
        return descriptors;
    }

    public ICollection<IDrawCallDescriptor> GetDrawCallDescriptors(IAssetManager assets)
    {
        var descriptors = new List<IDrawCallDescriptor>();

        var inject = Area.Rooms.FirstOrDefault();
        if (inject is not null)
        {
            Projection.ForEach(x => inject.AddObject(x));
        }
        else
        {
            Projection.ForEach(x => RenderObject(assets, x, ref descriptors));
        }

        foreach (var room in Area.Rooms)
        {
            RenderRoom(assets, room, ref descriptors);
        }

        RenderMagnets(assets, ref descriptors);

        if (inject is not null)
        {
            Projection.ForEach(x => inject.Objects.Remove(x));
        }

        return descriptors;
    }
    public void RenderRoom(IAssetManager assets, Room room, ref List<IDrawCallDescriptor> descriptors)
    {
        foreach (var @object in room.Objects)
        {
            RenderObject(assets, @object, ref descriptors);
        }

        descriptors.RemoveAll(x => x is MeshDescriptor mesh && mesh.TransparencyHint != 0);
    }
    public void RenderObject(IAssetManager assets, WorldObject worldObject, ref List<IDrawCallDescriptor> descriptors)
    {
        if (!worldObject.Visible)
            return;
        if (worldObject.Type == WorldObjectType.Ceiling && !DoRenderCeiling)
            return;
        if (worldObject.Type == WorldObjectType.Wall && !DoRenderWalls)
            return;
        if (worldObject.Type == WorldObjectType.Floor && !DoRenderFloor)
            return;
        if (worldObject.Type == WorldObjectType.DoorFrame && false)
            return;
        if (worldObject.Type == WorldObjectType.OuterCorner && !DoRenderWalls)
            return;
        if (worldObject.Type == WorldObjectType.InnerCorner && !DoRenderWalls)
            return;
        if (worldObject.Type == WorldObjectType.Generic && !DoRenderObjects)
            return;

        foreach (var attachedWorldObject in worldObject.AttachedObjects)
        {
            RenderObject(assets, attachedWorldObject, ref descriptors);
        }

        if (!string.IsNullOrWhiteSpace(worldObject.Template.Model))
        {
            descriptors.AddRange(DescriptorsForModel(assets, $"{worldObject.Template.KitID}::{worldObject.Template.Model}", worldObject.GlobalTransform, worldObject));
        }
    }
    public void RenderMagnets(IAssetManager assets, ref List<IDrawCallDescriptor> descriptors)
    {
        var size = (0.4f) + MathF.Sin((RunningTime % 0.75f) / 0.75f * MathF.PI) * 0.2f;
        descriptors.AddRange(Area.AvailableMagnets
            .Where(x => x.IsTileMagnet)
            .ToList()
            .Select(magnet => new BillboardDescriptor()
            {
                AllwaysOnTop = true,
                DoRender = true,
                FixedSize = false,
                Image = "magnet",
                Location = magnet.GlobalPosition,
                Size = size
            }));
    }

    // TODO - clean this up somehow
    private ICollection<IDrawCallDescriptor> DescriptorsForModel(IAssetManager assets, string modelName, Matrix4x4 transform, object tag = null)
    {
        var model = assets.GetModel(modelName);
        model.Root.GenerateTransform([]);
        return model.GetAllNodes()
            .SelectMany(node => node.GetDrawCallDescriptors(transform))
            .Select(x =>
            {
                x.Tag = tag;
                return x;
            })
            .ToList();
    }
}
