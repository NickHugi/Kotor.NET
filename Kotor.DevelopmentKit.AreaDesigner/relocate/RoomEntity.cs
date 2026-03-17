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

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

public class RoomEntity : BaseEntity
{
    public Room Room { get; }

    public RoomEntity(Room room)
    {
        Room = room;
    }

    public override ICollection<MeshDescriptor> GetMeshDescriptors(IAssetManager assets)
    {
        List<MeshDescriptor> descriptors = [];

        foreach (var tile in Room.GetAllTiles())
        {
            descriptors.AddRange(DescriptorsForModel(assets, tile.Floor.Model, tile.Transform));

            foreach (var wall in tile.Walls)
            {
                if (wall.LinkedTile is not null)
                    continue;

                var transform = Matrix4x4.CreateFromQuaternion(wall.Template.Orientation)
                    * Matrix4x4.CreateTranslation(wall.Template.Position);
                descriptors.AddRange(DescriptorsForModel(assets, wall.Model, transform * tile.Transform));
            }
            foreach (var corner in tile.InnerCorners)
            {
                if (tile.Walls.ElementAt(corner.Requires.IndexA).LinkedTile is not null)
                    continue;
                if (tile.Walls.ElementAt(corner.Requires.IndexB).LinkedTile is not null)
                    continue;

                descriptors.AddRange(DescriptorsForModel(assets, corner.Model, corner.Transform * tile.Transform));
            }
            foreach (var corner in tile.OuterCorners)
            {
                if (tile.Walls.ElementAt(corner.Requires.IndexA).LinkedTile is null)
                    continue;
                if (tile.Walls.ElementAt(corner.Requires.IndexB).LinkedTile is null)
                    continue;

                descriptors.AddRange(DescriptorsForModel(assets, corner.Model, corner.Transform * tile.Transform));
            }
        }

        return descriptors;
    }

    public override void Update(IAssetManager assetManager, float delta)
    {

    }


    public ICollection<MeshDescriptor> DescriptorsForModel(IAssetManager assets, string modelName, Matrix4x4 transform)
    {
        var model = assets.GetModel(modelName);
        model.Root.GenerateTransform([]);
        return model.GetAllNodes().ToList().SelectMany(node => node.GetMeshDescriptors(transform)).ToList();
    }
}
