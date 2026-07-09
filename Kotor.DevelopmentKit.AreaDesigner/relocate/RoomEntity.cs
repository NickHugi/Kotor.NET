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
    public void RenderObject(IAssetManager assets, UltimateWorldObject worldObject, ref List<IDrawCallDescriptor> descriptors)
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
        if (worldObject.Type == WorldObjectType.Prop && !DoRenderObjects)
            return;

        foreach (var attachedWorldObject in worldObject.AttachedObjects)
        {
            RenderObject(assets, attachedWorldObject, ref descriptors);
        }

        if (!string.IsNullOrWhiteSpace(worldObject.Template.Model))
        {
            descriptors.AddRange(DescriptorsForModel(assets, worldObject.Template.Model, worldObject.GlobalTransform, worldObject));
        }

        // TODO
        //foreach (var doorframe in tile.AttachedObjects.OfType<UltimateWorldObject>().Select(x => x.DoorFrame).Where(x => x is not null))
        //{
        //    RenderDoorFrame(assets, doorframe, ref descriptors);
        //}
    }
    private void RenderWall(IAssetManager assets, UltimateWorldObject wall, ref List<IDrawCallDescriptor> descriptors)
    {
        // TODO
        //if (!wall.Visible || ((wall.DoorFrame is null && !DoRenderWalls) || (wall.DoorFrame is not null && !DoRenderDoors)))
        //    return;

        //descriptors.AddRange(DescriptorsForModel(assets, wall.Template.Model, wall.GlobalTransform, wall));
    }
    private void RenderDoorFrame(IAssetManager assets, DoorFrame doorframe, ref List<IDrawCallDescriptor> descriptors)
    {
        if (!doorframe.Visible || !DoRenderDoors)
            return;

        descriptors.AddRange(DescriptorsForModel(assets, doorframe.Template.Model, doorframe.GlobalTransform, doorframe));
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
