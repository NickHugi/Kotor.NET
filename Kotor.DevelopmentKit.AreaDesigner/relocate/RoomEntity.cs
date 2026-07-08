using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.Renderers.Descriptors;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

public class AreaEntity : BaseEntity
{
    public Area Area { get; set; } = new();

    public bool DoRenderCorners { get; set; } = true;
    public bool DoRenderWalls { get; set; } = true;
    public bool DoRenderDoors { get; set; } = true;
    public bool DoRenderFloor { get; set; } = true;
    public bool DoRenderCeiling { get; set; } = false;
    public bool DoRenderObjects { get; set; } = true;

    public override ICollection<IDrawCallDescriptor> GetDrawCallDescriptors(IAssetManager assets)
    {
        var descriptors = new List<IDrawCallDescriptor>();

        foreach (var room in Area.Rooms)
        {
            RenderRoom(assets, room, ref descriptors);
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
    public void RenderObject(IAssetManager assets, UltimateWorldObject obj, ref List<IDrawCallDescriptor> descriptors)
    {
        if (!DoRenderObjects)
            return;

        if (obj is Tile tile)
        {
            foreach (var worldObject in tile.AttachedObjects.Where(x => typeof(UltimateWorldObject) == x.GetType()))
            {
                descriptors.AddRange(DescriptorsForModel(assets, worldObject.Template.Model, obj.GlobalTransform, obj));
            }

            foreach (var ceiling in tile.AttachedObjects.OfType<Ceiling>())
            {
                RenderCeiling(assets, ceiling, ref descriptors);
            }
            foreach (var wall in tile.AttachedObjects.OfType<Wall>())
            {
                RenderWall(assets, wall, ref descriptors);
            }
            foreach (var doorframe in tile.AttachedObjects.OfType<Wall>().Select(x => x.DoorFrame).Where(x => x is not null))
            {
                RenderDoorFrame(assets, doorframe, ref descriptors);
            }
            foreach (var innerCorner in tile.AttachedObjects.OfType<InnerCorner>())
            {
                RenderInnerCorner(assets, innerCorner, ref descriptors);
            }
            foreach (var outerCorner in tile.AttachedObjects.OfType<OuterCorner>())
            {
                RenderOuterCorner(assets, outerCorner, ref descriptors);
            }
        }
        if (obj is WorldObject prop)
        {
            descriptors.AddRange(DescriptorsForModel(assets, prop.Template.Model, obj.GlobalTransform, obj));
        }
    }
    private void RenderCeiling(IAssetManager assets, Ceiling ceiling, ref List<IDrawCallDescriptor> descriptors)
    {
        if (!DoRenderCeiling)
            return;

        descriptors.AddRange(DescriptorsForModel(assets, ceiling.Template.Model, ceiling.GlobalTransform, ceiling));
    }
    private void RenderWall(IAssetManager assets, Wall wall, ref List<IDrawCallDescriptor> descriptors)
    {
        if (!wall.Visible || ((wall.DoorFrame is null && !DoRenderWalls) || (wall.DoorFrame is not null && !DoRenderDoors)))
            return;

        descriptors.AddRange(DescriptorsForModel(assets, wall.Template.Model, wall.GlobalTransform, wall));
    }
    private void RenderDoorFrame(IAssetManager assets, DoorFrame doorframe, ref List<IDrawCallDescriptor> descriptors)
    {
        if (!doorframe.Visible || !DoRenderDoors)
            return;

        descriptors.AddRange(DescriptorsForModel(assets, doorframe.Template.Model, doorframe.GlobalTransform, doorframe));
    }
    private void RenderInnerCorner(IAssetManager assets, InnerCorner corner, ref List<IDrawCallDescriptor> descriptors)
    {
        if (!corner.Visible || !DoRenderCorners)
            return;

        descriptors.AddRange(DescriptorsForModel(assets, corner.Template.Model, corner.GlobalTransform, corner));
    }
    private void RenderOuterCorner(IAssetManager assets, OuterCorner corner, ref List<IDrawCallDescriptor> descriptors)
    {
        if (!corner.Visible || !DoRenderCorners)
            return;

        descriptors.AddRange(DescriptorsForModel(assets, corner.Template.Model, corner.GlobalTransform, corner));
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

    public override void Update(IAssetManager assetManager, float delta)
    {

    }
}
