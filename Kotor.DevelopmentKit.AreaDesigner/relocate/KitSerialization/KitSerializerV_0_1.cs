using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Numerics;
using Kotor.NET.Graphics.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;

public class KitSerializerV_0_1
{
    public const string FormatID = "0.1";

    public static Kit Load(string filepath)
    {
        var json = File.ReadAllText(filepath);
        dynamic data = JsonConvert.DeserializeObject(json);

        string kitName = data.name.Value;
        string kitID = data.id.Value;
        int kitVersion = (int)data.version.Value;

        if (kitID != Path.GetFileNameWithoutExtension(filepath))
            throw new ArgumentException($"Kit ID {kitID} does not match filename {Path.GetFileName(filepath)}.");

        var kit = new Kit(filepath, kitID, kitVersion, kitName);

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
                    Orientation = ((float[])hook.orientation.ToObject<float[]>()).ToQuaternion()
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
                DoorFrameID = wall.doorframeID?.Value,
            });
        }

        foreach (var tile in data.tiles)
        {
            kit.Tiles.Add(new TileTemplate
            {
                ID = tile.id.Value,
                Name = tile.name.Value,
                DefaultFloorID = tile.defaultFloorID.Value,
                DefaultCeilingID = tile.defaultCeilingID?.Value ?? "",
                Walls = ((JArray)tile.wallHooks).Select(x => (dynamic)x).Select(hook => new WallHookTemplate
                {
                    DefaultWallID = hook.defaultWallID,
                    LocalPosition = new Vector3(hook.position.ToObject<float[]>()),
                    LocalOrientation = ((float[])hook.orientation.ToObject<float[]>()).ToQuaternion()
                }).ToArray(),
                InnerCorners = ((JArray)tile.innerCornerHooks).Select(x => (dynamic)x).Select(hook => new CornerTemplate
                {
                    ID = hook.defaultInnerCornerID.Value,
                    Adjacent = hook.adjacencies?.ToObject<int[]>() ?? new int[0],
                    Position = new Vector3(hook.position.ToObject<float[]>()),
                    Orientation = ((float[])hook.orientation.ToObject<float[]>()).ToQuaternion()
                }).ToArray(),
                OuterCorners = ((JArray)tile.outerCornerHooks).Select(x => (dynamic)x).Select(hook => new CornerTemplate
                {
                    ID = hook.defaultOuterCornerID.Value,
                    Adjacent = hook.adjacencies?.ToObject<int[]>() ?? new int[0],
                    Position = new Vector3(hook.position.ToObject<float[]>()),
                    Orientation = ((float[])hook.orientation.ToObject<float[]>()).ToQuaternion()
                }).ToArray(),
                CeilingHooks = []
            });
        }

        foreach (var @object in data.objects)
        {
            kit.Objects.Add(new ObjectTemplate
            {
                ID = @object.id.Value,
                Name = @object.name.Value,
                Model = @object.model.Value,
            });
        }

        return kit;
    }

    public static void Save(string filepath, Kit kit)
    {
        dynamic data = new ExpandoObject();

        data.id = kit.ID;
        data.version = kit.Version;
        data.name = kit.Name;
        data.format = FormatID;

        data.tiles = kit.Tiles.Select(tile => new
        {
            id = tile.ID,
            name = tile.Name,
            defaultFloorID = tile.DefaultFloorID,
            defaultCeilingID = tile.DefaultCeilingID,
            wallHooks = tile.Walls.Select(x => new
            {
                defaultWallID = x.DefaultWallID,
                position = x.LocalPosition.ToFloatArray(),
                orientation = x.LocalOrientation.ToFloatArray(),
            }),
            innerCornerHooks = tile.InnerCorners.Select(x => new
            {
                defaultInnerCornerID = x.ID,
                position = x.Position.ToFloatArray(),
                orientation = x.Orientation.ToFloatArray(),
                adjacencies = x.Adjacent,
            }),
            outerCornerHooks = tile.OuterCorners.Select(x => new
            {
                defaultOuterCornerID = x.ID,
                position = x.Position.ToFloatArray(),
                orientation = x.Orientation.ToFloatArray(),
                adjacencies = x.Adjacent,
            }),
        });

        data.floors = kit.Floors.Select(floor => new
        {
            id = floor.ID,
            name = floor.Name,
            model = floor.Model,
        });

        data.doorframes = kit.DoorFrames.Select(doorframe => new
        {
            id = doorframe.ID,
            name = doorframe.Name,
            model = doorframe.Model,
            hooks = doorframe.Hooks.Select(hook => new
            {
                position = hook.Position.ToFloatArray(),
                orientation = hook.Orientation.ToFloatArray(),
            })
        });

        data.walls = kit.Walls.Select(wall => new
        {
            id = wall.ID,
            name = wall.Name,
            model = wall.Model,
            doorframeID = wall.DoorFrameID,
        });

        data.objects = kit.Objects.Select(obj => new
        {
            id = obj.ID,
            name = obj.Name,
            model = obj.Model,
        });

        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filepath, json);
    }
}
