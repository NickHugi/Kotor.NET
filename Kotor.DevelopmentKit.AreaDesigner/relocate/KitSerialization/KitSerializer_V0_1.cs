using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
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
        dynamic data = JsonConvert.DeserializeObject(json)!;

        string kitName = data.name.Value;
        string kitID = data.id.Value;
        int kitVersion = (int)data.version.Value;

        if (kitID != Path.GetFileNameWithoutExtension(filepath))
            throw new ArgumentException($"Kit ID {kitID} does not match filename {Path.GetFileName(filepath)}.");

        var kit = new Kit(filepath, kitID, kitVersion, kitName);

        foreach (var worldObject in data.worldObjects)
        {
            kit.Objects.Add(new WorldObjectTemplate
            {
                Type = (WorldObjectType)worldObject.type.Value,
                KitID = kitID,
                TemplateID = worldObject.templateID.Value,
                ClassID = worldObject.classID.Value,
                Name = worldObject.name.Value,
                Model = worldObject.model.Value,
                Magnets = ((JArray)worldObject.magnets).Select(DeserializeMagnet).ToArray(),
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
        data.worldObjects = kit.Objects.Select(worldObject => new
        {
            templateID = worldObject.TemplateID,
            classID = worldObject.ClassID,
            name = worldObject.Name,
            model = worldObject.Model,
            type = worldObject.Type,
            magnets = worldObject.Magnets.Select(SerializeMagnet),
        });

        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filepath, json);
    }

    private static MagnetTemplate DeserializeMagnet(JToken data)
    {
        var tag = data.Value<string>("tag") ?? "";
        var position = new Vector3(data["position"]?.ToObject<float[]>() ?? [0, 0, 0]);
        var orientation = (data["orientation"]?.ToObject<float[]>() ?? [0, 0, 0, 1]).ToQuaternion();
        var scale = new Vector3(data["scale"]?.ToObject<float[]>() ?? [1, 1, 1]);
        var serializedMagnetType = data.Value<int?>("magnetType");
        var legacyKitID = data.Value<string>("kitID") ?? "";
        var legacyTemplateID = data.Value<string>("templateID") ?? "";
        var magnetType = serializedMagnetType.HasValue
            ? (MagnetType)serializedMagnetType.Value
            : data.Value<string>("templateType") switch
            {
                "targetHook" => MagnetType.TargetHook,
                "sourceHook" => MagnetType.SourceHook,
                _ => MagnetType.Magnet,
            };

        return magnetType switch
        {
            MagnetType.TargetHook => new TargetHookTemplate
            {
                Tag = tag,
                LocalPosition = position,
                LocalOrientation = orientation,
                LocalScale = scale,
                MagnetType = magnetType,
            },
            MagnetType.SourceHook => new SourceHookTemplate
            {
                Tag = tag,
                KitID = legacyKitID,
                C1TemplateID = data.Value<string>("c1TemplateID") ?? legacyTemplateID,
                C2TemplateID = data.Value<string>("c2TemplateID") ?? "",
                C3TemplateID = data.Value<string>("c3TemplateID") ?? "",
                C4TemplateID = data.Value<string>("c4TemplateID") ?? "",
                LocalPosition = position,
                LocalOrientation = orientation,
                LocalScale = scale,
                MagnetType = magnetType,
            },
            _ => new MagnetTemplate
            {
                Tag = tag,
                LocalPosition = position,
                LocalOrientation = orientation,
                LocalScale = scale,
                MagnetType = magnetType,
            },
        };
    }

    private static JObject SerializeMagnet(MagnetTemplate magnet)
    {
        var magnetType = magnet switch
        {
            TargetHookTemplate => MagnetType.TargetHook,
            SourceHookTemplate => MagnetType.SourceHook,
            _ => MagnetType.Magnet,
        };
        var data = new JObject
        {
            ["tag"] = magnet.Tag,
            ["position"] = JToken.FromObject(magnet.LocalPosition.ToFloatArray()),
            ["orientation"] = JToken.FromObject(magnet.LocalOrientation.ToFloatArray()),
            ["scale"] = JToken.FromObject(magnet.LocalScale.ToFloatArray()),
            ["magnetType"] = (int)magnetType,
        };

        if (magnet is SourceHookTemplate sourceHook)
        {
            data["kitID"] = sourceHook.KitID;
            data["c1TemplateID"] = sourceHook.C1TemplateID;
            data["c2TemplateID"] = sourceHook.C2TemplateID;
            data["c3TemplateID"] = sourceHook.C3TemplateID;
            data["c4TemplateID"] = sourceHook.C4TemplateID;
        }

        return data;
    }
}
