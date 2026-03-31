using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

public class KitLoader
{
    public static Kit Load(string filepath)
    {
        var json = File.ReadAllText(filepath);
        dynamic data = JsonConvert.DeserializeObject(json);

        var kit = new Kit(data.name.Value);

        foreach (var floor in data.floors)
        {
            kit.Floors.Add(new FloorTemplate
            {
                ID = floor.id.Value,
                Name = floor.name.Value,
                Model = floor.model.Value,
            });
        }

        foreach (var door in data.doorframes)
        {
            kit.DoorFrames.Add(new DoorFrameTemplate
            {
                ID = door.id.Value,
                Name = door.name.Value,
                Model = door.model.Value,
                Hooks = ((JArray)door.hooks).Select(x => (dynamic)x).Select(hook => new DoorFrameHookTemplate
                {
                    Position = new Vector3(hook.position.ToObject<float[]>()),
                    Orientation = QuaternionFromArray(hook.orientation.ToObject<float[]>())
                }).ToArray()
            });
        }

        foreach (var wall in data.walls)
        {
            kit.Walls.Add(new WallTemplate
            {
                ID = wall.id.Value,
                Name = wall.name.Value,
                Model = wall.model.Value,
                DoorFrameID = wall.doorframe?.Value,
            });
        }

        foreach (var tile in data.tiles)
        {
            kit.Tiles.Add(new TileTemplate
            {
                ID = tile.id.Value,
                Name = tile.name.Value,
                DefaultFloorID = tile.defaultFloor.Value,
                DefaultCeilingID = "", // todo - tile.defaultCeiling.Value,
                Walls = ((JArray)tile.walls).Select(x => (dynamic)x).Select(hook => new WallHookTemplate
                {
                    DefaultWallID = hook.defaultWall,
                    LocalPosition = new Vector3(hook.position.ToObject<float[]>()),
                    LocalOrientation = QuaternionFromArray(hook.orientation.ToObject<float[]>())
                }).ToArray(),
                InnerCorners = ((JArray)tile.insideCorner).Select(x => (dynamic)x).Select(hook => new CornerTemplate
                {
                    ID = hook.id.Value,
                    Adjacent = hook.adjacent?.ToObject<int[]>() ?? new int[0],
                    Position = new Vector3(hook.position.ToObject<float[]>()),
                    Orientation = QuaternionFromArray(hook.orientation.ToObject<float[]>())
                }).ToArray(),
                OuterCorners = ((JArray)tile.outsideCorner).Select(x => (dynamic)x).Select(hook => new CornerTemplate
                {
                    ID = hook.id.Value,
                    Adjacent = hook.adjacent?.ToObject<int[]>() ?? new int[0],
                    Position = new Vector3(hook.position.ToObject<float[]>()),
                    Orientation = QuaternionFromArray(hook.orientation.ToObject<float[]>())
                }).ToArray(),
                CeilingHooks = []
            });
        }

        foreach (var @object in data.objects)
        {
            kit.Objects.Add(new ObjectTemplate
            {
                ID = @object.id.Value,
                Model = @object.model.Value,
            });
        }

        return kit;
    }

    private static Quaternion QuaternionFromArray(float[] array)
    {
        return new Quaternion(array[0], array[1], array[2], array[3]);
    }
}
