using System.Dynamic;
using System.IO;
using System.Linq;
using System.Numerics;
using Kotor.NET.Graphics.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

public class KitLoaderV_0_1
{
    public static Kit Load(string filepath)
    {
        var json = File.ReadAllText(filepath);
        dynamic data = JsonConvert.DeserializeObject(json);

        var kit = new Kit(filepath, data.id.Value, data.name.Value);

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
                    LocalOrientation = ((float[])hook.orientation.ToObject<float[]>()).ToQuaternion()
                }).ToArray(),
                InnerCorners = ((JArray)tile.insideCorner).Select(x => (dynamic)x).Select(hook => new CornerTemplate
                {
                    ID = hook.id.Value,
                    Adjacent = hook.adjacent?.ToObject<int[]>() ?? new int[0],
                    Position = new Vector3(hook.position.ToObject<float[]>()),
                    Orientation = ((float[])hook.orientation.ToObject<float[]>()).ToQuaternion()
                }).ToArray(),
                OuterCorners = ((JArray)tile.outsideCorner).Select(x => (dynamic)x).Select(hook => new CornerTemplate
                {
                    ID = hook.id.Value,
                    Adjacent = hook.adjacent?.ToObject<int[]>() ?? new int[0],
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

        data.tiles = kit.Tiles.Select(tile => new
        {
            id = tile.ID,
            name = tile.Name,
            defaultFloorID = tile.DefaultFloorID,
            defaultCeilingID = tile.DefaultCeilingID,
            walls = tile.Walls.Select(x => new
            {
                defaultWall = x.DefaultWallID,
                position = x.LocalPosition,
                orientation = x.LocalOrientation,
            }),
            insideCorner = tile.InnerCorners.Select(x => new
            {
                id = x.ID,
                position = x.Position,
                orientation = x.Orientation,
                adjacent = x.Adjacent,
            }),
            outsideCorner = tile.OuterCorners.Select(x => new
            {
                id = x.ID,
                position = x.Position,
                orientation = x.Orientation,
                adjacent = x.Adjacent,
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
                position = hook.Position,
                orientation = hook.Orientation,
            })
        });

        data.walls = kit.Walls.Select(wall => new
        {
            id = wall.ID,
            name = wall.Name,
            model = wall.Model,
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

public class KitLoader
{
    public static Kit Load(string filepath)
    {
        return KitLoaderV_0_1.Load(filepath);
    }

    public static void Save(string filepath, Kit kit)
    {
        KitLoaderV_0_1.Save(filepath, kit);
    }
}
