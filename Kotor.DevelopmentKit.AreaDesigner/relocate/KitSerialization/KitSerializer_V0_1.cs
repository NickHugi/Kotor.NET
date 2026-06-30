using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;
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
            kit.Objects.Add(new FloorTemplate
            {
                KitID = kitID,
                TemplateID = floor.templateID.Value,
                ClassID = floor.classID.Value,
                Name = floor.name.Value,
                Model = floor.model.Value,
                Magnets = []
            });
        }

        foreach (var ceiling in data.ceilings)
        {
            kit.Objects.Add(new CeilingTemplate
            {
                KitID = kitID,
                TemplateID = ceiling.templateID.Value,
                ClassID = ceiling.classID.Value,
                Name = ceiling.name.Value,
                Model = ceiling.model.Value,
                Magnets = []
            });
        }

        foreach (var door in data.doorframes)
        {
            kit.Objects.Add(new DoorFrameTemplate
            {
                KitID = kitID,
                TemplateID = door.templateID.Value,
                ClassID = door.classID.Value,
                Name = door.name.Value,
                Model = door.model.Value,
                Magnets = ((JArray)door.hooks).Select(x => (dynamic)x).Select(hook => new DoorFrameHookTemplate
                {
                    LocalPosition = new Vector3(hook.position.ToObject<float[]>()),
                    LocalOrientation = ((float[])hook.orientation.ToObject<float[]>()).ToQuaternion()
                }).ToArray()
            });
        }

        foreach (var wall in data.walls)
        {
            kit.Objects.Add(new WallTemplate
            {
                KitID = kitID,
                TemplateID = wall.templateID.Value,
                ClassID = wall.classID.Value,
                Name = wall.name.Value,
                Model = wall.model.Value,
                DoorframeKitID = wall.doorframeKtID?.Value ?? "",
                DoorframeTemplateID = wall.doorframeTemplateID?.Value ?? "",
                DoorframeClassID = wall.doorframeClassID?.Value ?? "",
                Magnets = []
            });
        }

        foreach (var tile in data.tiles)
        {
            kit.Objects.Add(new TileTemplate
            {
                KitID = kitID,
                TemplateID = tile.templateID.Value,
                Name = tile.name.Value,
                ClassID = "",
                Model = "",
                Magnets =
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
                    ..((JArray)tile.cornerHooks).Select(x => (dynamic)x).Select(hook => new CornerHookTemplate
                    {
                        InnerKitID = hook.innerKitID,
                        InnerTemplateID = hook.innerTemplateID,
                        OuterKitID = hook.outerKitID,
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
            kit.Objects.Add(new InnerCornerTemplate
            {
                KitID = kitID,
                TemplateID = innerCorner.templateID.Value,
                ClassID = innerCorner.classID.Value,
                Name = innerCorner.name.Value,
                Model = innerCorner.model.Value,
                Magnets = []
            });
        }

        foreach (var outerCorner in data.outerCorners)
        {
            kit.Objects.Add(new OuterCornerTemplate
            {
                KitID = kitID,
                TemplateID = outerCorner.templateID.Value,
                ClassID = outerCorner.classID.Value,
                Name = outerCorner.name.Value,
                Model = outerCorner.model.Value,
                Magnets = []
            });
        }

        foreach (var @object in data.objects)
        {
            kit.Objects.Add(new PropTemplate
            {
                KitID = kitID,
                TemplateID = @object.templateID.Value,
                ClassID = @object.classID.Value,
                Name = @object.name.Value,
                Model = @object.model.Value,
                Magnets = []
            });
        }

        return kit;
    }

    public static void Save(string filepath, Kit kit)
    {
        dynamic data = new ExpandoObject();

        data.id = kit.KitID;
        data.version = kit.Version;
        data.name = kit.Name;
        data.format = FormatID;

        data.tiles = kit.Objects.OfType<TileTemplate>().Select(tile => new
        {
            templateID = tile.TemplateID,
            classID = tile.ClassID,
            name = tile.Name,
            model = tile.Model,
            floorHooks = tile.Magnets.OfType<FloorHookTemplate>().Select(x => new
            {
                kitID = x.KitID,
                templateID = x.TemplateID,
                position = x.LocalPosition.ToFloatArray(),
                orientation = x.LocalOrientation.ToFloatArray()
            }),
            ceilingHooks = tile.Magnets.OfType<CeilingHookTemplate>().Select(x => new
            {
                kitID = x.KitID,
                templateID = x.TemplateID,
                position = x.LocalPosition.ToFloatArray(),
                orientation = x.LocalOrientation.ToFloatArray()
            }),
            wallHooks = tile.Magnets.OfType<WallHookTemplate>().Select(x => new
            {
                kitID = x.KitID,
                templateID = x.TemplateID,
                position = x.LocalPosition.ToFloatArray(),
                orientation = x.LocalOrientation.ToFloatArray(),
            }),
            cornerHooks = tile.Magnets.OfType<CornerHookTemplate>().Select(x => new
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

        data.floors = kit.Objects.OfType<FloorTemplate>().Select(floor => new
        {
            templateID = floor.TemplateID,
            classID = floor.ClassID,
            name = floor.Name,
            model = floor.Model,
        });

        data.ceilings = kit.Objects.OfType<CeilingTemplate>().Select(ceiling => new
        {
            templateID = ceiling.TemplateID,
            classID = ceiling.ClassID,
            name = ceiling.Name,
            model = ceiling.Model,
        });

        data.doorframes = kit.Objects.OfType<DoorFrameTemplate>().Select(doorframe => new
        {
            templateID = doorframe.TemplateID,
            classID = doorframe.ClassID,
            name = doorframe.Name,
            model = doorframe.Model,
            hooks = doorframe.Magnets.Select(hook => new
            {
                position = hook.LocalPosition.ToFloatArray(),
                orientation = hook.LocalOrientation.ToFloatArray(),
            })
        });

        data.walls = kit.Objects.OfType<WallTemplate>().Select(wall => new
        {
            templateID = wall.TemplateID,
            classID = wall.ClassID,
            name = wall.Name,
            model = wall.Model,
            doorframeKitID = wall.DoorframeKitID,
            doorframeTemplateID = wall.DoorframeTemplateID,
            doorframeClassID = wall.DoorframeClassID,
        });

        data.innerCorners = kit.Objects.OfType<InnerCornerTemplate>().Select(obj => new
        {
            templateID = obj.TemplateID,
            classID = obj.ClassID,
            name = obj.Name,
            model = obj.Model,
        });

        data.outerCorners = kit.Objects.OfType<OuterCornerTemplate>().Select(obj => new
        {
            templateID = obj.TemplateID,
            classID = obj.ClassID,
            name = obj.Name,
            model = obj.Model,
        });

        data.objects = kit.Objects.Where(x => x.GetType() == typeof(WorldObjectTemplate)).Select(obj => new
        {
            templateID = obj.TemplateID,
            classID = obj.ClassID,
            name = obj.Name,
            model = obj.Model,
        });

        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filepath, json);
    }
}
