using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.NET.Graphics.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;

public class KitSerializer_V0_1
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
                KitID = floor.kitID.Value,
                TemplateID = floor.templateID.Value,
                Name = floor.name.Value,
                ClassID = floor.group?.Value,
                Model = floor.model.Value,
                Hooks = []
            });
        }

        foreach (var ceiling in data.ceilings)
        {
            kit.Ceilings.Add(new CeilingTemplate
            {
                KitID = ceiling.kitID.Value,
                TemplateID = ceiling.templateID.Value,
                Name = ceiling.name.Value,
                ClassID = ceiling.group?.Value,
                Model = ceiling.model.Value,
                Hooks = []
            });
        }

        foreach (var door in data.doorframes)
        {
            kit.DoorFrames.Add(new DoorFrameTemplate
            {
                KitID = door.kitID.Value,
                TemplateID = door.templateID.Value,
                Name = door.name.Value,
                ClassID = door.group?.Value,
                Model = door.model.Value,
                Hooks = ((JArray)door.hooks).Select(x => (dynamic)x).Select(hook => new DoorFrameHookTemplate
                {
                    LocalPosition = new Vector3(hook.position.ToObject<float[]>()),
                    LocalOrientation = ((float[])hook.orientation.ToObject<float[]>()).ToQuaternion()
                }).ToArray()
            });
        }

        foreach (var wall in data.walls)
        {
            kit.Walls.Add(new WallTemplate
            {
                KitID = wall.kitID.Value,
                TemplateID = wall.templateID.Value,
                Name = wall.name.Value,
                Model = wall.model.Value,
                ClassID = wall.group.Value,
                DoorFrameID = wall.doorframeID?.Value,
                Hooks = []
            });
        }

        foreach (var tile in data.tiles)
        {
            kit.Tiles.Add(new TileTemplate
            {
                KitID = tile.kitID.Value,
                TemplateID = tile.templateID.Value,
                Name = tile.name.Value,
                ClassID = null,
                Model = null,
                Hooks =
                [
                    ..((JArray)tile.floorHooks).Select(x => (dynamic)x).Select(hook => new FloorHookTemplate
                    {
                        KitID = hook.kitID,
                        TemplateID = hook.templateID,
                        LocalPosition = new Vector3(hook.position.ToObject<float[]>()),
                        LocalOrientation = ((float[])hook.orientation.ToObject<float[]>()).ToQuaternion()
                    }),
                    ..((JArray)tile.ceilingHooks).Select(x => (dynamic)x).Select(hook => new CeilingHookTemplate
                    {
                        KitID = hook.kitID,
                        TemplateID = hook.templateID,
                        LocalPosition = new Vector3(hook.position.ToObject<float[]>()),
                        LocalOrientation = ((float[])hook.orientation.ToObject<float[]>()).ToQuaternion()
                    }),
                    ..((JArray)tile.wallHooks).Select(x => (dynamic)x).Select(hook => new WallHookTemplate
                    {
                        KitID = hook.kitID,
                        TemplateID = hook.templateID,
                        LocalPosition = new Vector3(hook.position.ToObject<float[]>()),
                        LocalOrientation = ((float[])hook.orientation.ToObject<float[]>()).ToQuaternion()
                    }),
                    ..((JArray)tile.wallHooks).Select(x => (dynamic)x).Select(hook => new CornerHookTemplate
                    {
                        InnerKitID = hook.innerKitID,
                        InnerTemplateID = hook.innerTemplateID,
                        OuterKitID = hook.outertKitID,
                        OuterTemplateID = hook.outerTemplateID,
                        LocalPosition = new Vector3(hook.position.ToObject<float[]>()),
                        LocalOrientation = ((float[])hook.orientation.ToObject<float[]>()).ToQuaternion(),
                        Adjacent = []
                    }),
                ],
            });
        }

        foreach (var innerCorner in data.innerCorners)
        {
            kit.InnerCorners.Add(new InnerCornerTemplate
            {
                KitID = innerCorner.kitID.Value,
                TemplateID = innerCorner.templateID.Value,
                Name = innerCorner.name.Value,
                ClassID = innerCorner.group?.Value,
                Model = innerCorner.model.Value,
                Hooks = []
            });
        }

        foreach (var outerCorner in data.outerCorners)
        {
            kit.OuterCorners.Add(new OuterCornerTemplate
            {
                KitID = outerCorner.kitID.Value,
                TemplateID = outerCorner.templateID.Value,
                Name = outerCorner.name.Value,
                ClassID = outerCorner.group?.Value,
                Model = outerCorner.model.Value,
                Hooks = []
            });
        }

        foreach (var @object in data.objects)
        {
            kit.Objects.Add(new ObjectTemplate
            {
                KitID = @object.kitID.Value,
                TemplateID = @object.templateID.Value,
                Name = @object.name.Value,
                ClassID = @object.group?.Value,
                Model = @object.model.Value,
                Hooks = []
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
            id = tile.TemplateID,
            name = tile.Name,
            floorHooks = tile.Hooks.OfType<FloorHookTemplate>().Select(x => new
            {
                kitID = x.KitID,
                templateID = x.TemplateID,
                position = x.LocalPosition.ToFloatArray(),
                orientation = x.LocalOrientation.ToFloatArray()
            }),
            ceilingHooks = tile.Hooks.OfType<CeilingHookTemplate>().Select(x => new
            {
                kitID = x.KitID,
                templateID = x.TemplateID,
                position = x.LocalPosition.ToFloatArray(),
                orientation = x.LocalOrientation.ToFloatArray()
            }),
            wallHooks = tile.Hooks.OfType<WallHookTemplate>().Select(x => new
            {
                kitID = x.KitID,
                templateID = x.TemplateID,
                position = x.LocalPosition.ToFloatArray(),
                orientation = x.LocalOrientation.ToFloatArray(),
            }),
            cornerHooks = tile.Hooks.OfType<CornerHookTemplate>().Select(x => new
            {
                innerKitID = x.InnerKitID,
                innerTemplateID = x.InnerTemplateID,
                outerKitID = x.OuterKitID,
                outerTemplateID = x.OuterTemplateID,
                position = x.LocalPosition.ToFloatArray(),
                orientation = x.LocalOrientation.ToFloatArray(),
                adjacencies = x.Adjacent,
            }),
        });

        data.floors = kit.Floors.Select(floor => new
        {
            id = floor.TemplateID,
            name = floor.Name,
            group = floor.ClassID,
            model = floor.Model,
        });

        data.ceilings = kit.Ceilings.Select(ceiling => new
        {
            id = ceiling.TemplateID,
            name = ceiling.Name,
            group = ceiling.ClassID,
            model = ceiling.Model,
        });

        data.doorframes = kit.DoorFrames.Select(doorframe => new
        {
            id = doorframe.TemplateID,
            name = doorframe.Name,
            group = doorframe.ClassID,
            model = doorframe.Model,
            hooks = doorframe.Hooks.Select(hook => new
            {
                position = hook.LocalPosition.ToFloatArray(),
                orientation = hook.LocalOrientation.ToFloatArray(),
            })
        });

        data.walls = kit.Walls.Select(wall => new
        {
            id = wall.TemplateID,
            name = wall.Name,
            model = wall.Model,
            group = wall.ClassID,
            doorframeID = wall.DoorFrameID,
        });

        data.innerCorners = kit.InnerCorners.Select(obj => new
        {
            id = obj.TemplateID,
            name = obj.Name,
            group = obj.ClassID,
            model = obj.Model,
        });

        data.outerCorners = kit.OuterCorners.Select(obj => new
        {
            id = obj.TemplateID,
            name = obj.Name,
            group = obj.ClassID,
            model = obj.Model,
        });

        data.objects = kit.Objects.Select(obj => new
        {
            id = obj.TemplateID,
            name = obj.Name,
            group = obj.ClassID,
            model = obj.Model,
        });

        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filepath, json);
    }
}
