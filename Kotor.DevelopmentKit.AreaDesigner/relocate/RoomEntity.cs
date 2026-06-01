using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.Model;
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

    public override ICollection<MeshDescriptor> GetMeshDescriptors(IAssetManager assets)
    {
        var descriptors = new List<MeshDescriptor>();

        foreach (var room in Area.Rooms)
        {
            RenderRoom(assets, room, ref descriptors);
        }

        return descriptors;
    }
    public void RenderRoom(IAssetManager assets, Room room, ref List<MeshDescriptor> descriptors)
    {
        foreach (var tile in room.Tiles)
        {
            RenderFloor(assets, tile, ref descriptors);
            RenderCeiling(assets, tile, ref descriptors);
        }
        foreach (var wall in room.Walls)
        {
            RenderWall(assets, wall, ref descriptors);
        }
        foreach (var doorframe in room.DoorFrames)
        {
            RenderDoorFrame(assets, doorframe, ref descriptors);
        }
        foreach (var corner in room.InnerCorners)
        {
            RenderInnerCorner(assets, corner, ref descriptors);
        }
        foreach (var corner in room.OuterCorners)
        {
            RenderOuterCorner(assets, corner, ref descriptors);
        }
        foreach (var @object in room.Objects)
        {
            RenderObject(assets, @object, ref descriptors);
        }
    }
    private void RenderFloor(IAssetManager assets, Tile tile, ref List<MeshDescriptor> descriptors)
    {
        if (!DoRenderFloor)
            return;

        descriptors.AddRange(DescriptorsForModel(assets, tile.Floor.Template.Model, tile.Transform, tile.Floor));
    }
    private void RenderCeiling(IAssetManager assets, Tile tile, ref List<MeshDescriptor> descriptors)
    {
        if (!DoRenderCeiling)
            return;

        descriptors.AddRange(DescriptorsForModel(assets, tile.Ceiling.Template.Model, tile.Transform, tile.Ceiling));
    }
    private void RenderWall(IAssetManager assets, Wall wall, ref List<MeshDescriptor> descriptors)
    {
        if (!wall.Visible || ((wall.DoorFrame is null && !DoRenderWalls) || (wall.DoorFrame is not null && !DoRenderDoors)))
            return;

        descriptors.AddRange(DescriptorsForModel(assets, wall.Template.Model, wall.Transform, wall));
    }
    private void RenderDoorFrame(IAssetManager assets, DoorFrame doorframe, ref List<MeshDescriptor> descriptors)
    {
        if (!doorframe.Visible || !DoRenderDoors)
            return;

        descriptors.AddRange(DescriptorsForModel(assets, doorframe.Template.Model, doorframe.Transform, doorframe));
    }
    private void RenderInnerCorner(IAssetManager assets, InnerCorner corner, ref List<MeshDescriptor> descriptors)
    {
        if (!corner.Visible || !DoRenderCorners)
            return;

        descriptors.AddRange(DescriptorsForModel(assets, corner.Template.Model, corner.Transform, corner));
    }
    private void RenderOuterCorner(IAssetManager assets, OuterCorner corner, ref List<MeshDescriptor> descriptors)
    {
        if (!corner.Visible || !DoRenderCorners)
            return;

        descriptors.AddRange(DescriptorsForModel(assets, corner.Template.Model, corner.Transform, corner));
    }
    public void RenderObject(IAssetManager assets, Object @object, ref List<MeshDescriptor> descriptors)
    {
        if (!DoRenderObjects)
            return;

        descriptors.AddRange(DescriptorsForModel(assets, @object.Template.Model, @object.LocalTransform, @object));
    }
    // TODO - clean this up somehow
    private ICollection<MeshDescriptor> DescriptorsForModel(IAssetManager assets, string modelName, Matrix4x4 transform, object tag = null)
    {
        var model = assets.GetModel(modelName);
        model.Root.GenerateTransform([]);
        return model.GetAllNodes()
            .SelectMany(node => node.GetMeshDescriptors(transform))
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
