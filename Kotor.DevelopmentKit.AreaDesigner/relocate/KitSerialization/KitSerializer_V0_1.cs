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
        dynamic data = JsonConvert.DeserializeObject(json);

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
                Magnets = ((JArray)worldObject.magnets).Select(x => (dynamic)x).Select(magnet => new MagnetTemplate
                {
                    KitID = magnet.kitID.Value,
                    TemplateID = magnet.templateID.Value,
                    LocalPosition = new Vector3(magnet.position.ToObject<float[]>()),
                    LocalOrientation = ((float[])magnet.orientation.ToObject<float[]>()).ToQuaternion(),
                    ConditionCheckLocalMagnetsOnly = magnet.conditionCheckLocalMagnetsOnly?.ToObject<bool>() ?? false,
                    ConditionMustHaveTemplate = magnet.conditionMustHaveTemplate?.ToObject<bool>() ?? false,
                    ConditionOverlapWillDisable = magnet.conditionOverlapWillDisable?.ToObject<bool>() ?? false,
                    ConditionOverlapCheckCount = magnet.conditionOverlapCheckCount?.ToObject<int>() ?? 0,
                    ConditionOverlapType = magnet.conditionOverlapType?.ToObject<OverlapCountType>() ?? OverlapCountType.Ignore,
                    ConditionOverlapOnlyEnableFirst = magnet.conditionOverlapOnlyEnableFirst?.ToObject<bool>() ?? false,
                    ConditionOverlapOnlyEnableMiddle = magnet.conditionOverlapOnlyEnableMiddle?.ToObject<bool>() ?? false,
                    ConditionOverlapOnlySameTemplate = magnet.conditionOverlapOnlySameTemplate?.ToObject<bool>() ?? false,
                    ConditionOverlapOnlySameClass = magnet.conditionOverlapOnlySameClass?.ToObject<bool>() ?? false,
                    ConditionOverlapOnlySameType = magnet.conditionOverlapOnlySameType?.ToObject<bool>() ?? false,
                    ConditionOverlapOnlySpecificTypes = magnet.conditionOverlapOnlySpecificTypes?.ToObject<WorldObjectType?[]?>()
                }).ToArray()
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
            magnets = worldObject.Magnets.Select(magnet => new
            {
                kitID = magnet.KitID,
                templateID = magnet.TemplateID,
                position = magnet.LocalPosition.ToFloatArray(),
                orientation = magnet.LocalOrientation.ToFloatArray(),
                conditionCheckLocalMagnetsOnly = magnet.ConditionCheckLocalMagnetsOnly,
                conditionMustHaveTemplate = magnet.ConditionMustHaveTemplate,
                conditionOverlapWillDisable = magnet.ConditionOverlapWillDisable,
                conditionOverlapCheckCount = magnet.ConditionOverlapCheckCount,
                conditionOverlapType = magnet.ConditionOverlapType,
                conditionOverlapOnlyEnableFirst = magnet.ConditionOverlapOnlyEnableFirst,
                conditionOverlapOnlyEnableMiddle = magnet.ConditionOverlapOnlyEnableMiddle,
                conditionOverlapOnlySameTemplate = magnet.ConditionOverlapOnlySameTemplate,
                conditionOverlapOnlySameClass = magnet.ConditionOverlapOnlySameClass,
                conditionOverlapOnlySameType = magnet.ConditionOverlapOnlySameType,
                conditionOverlapOnlySpecificTypes = magnet.ConditionOverlapOnlySpecificTypes
            })
        });

        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filepath, json);
    }
}
