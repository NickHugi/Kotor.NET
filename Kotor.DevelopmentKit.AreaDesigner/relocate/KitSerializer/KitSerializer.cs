using System;
using System.IO;
using Newtonsoft.Json;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerializer;

public class KitSerializer
{
    public static Kit Load(string filepath)
    {
        var json = File.ReadAllText(filepath);
        dynamic data = JsonConvert.DeserializeObject(json);
        string serializerVersion = (string)data.serializerVersion.Value;

        return serializerVersion switch
        {
            "0.1" => KitSerializerV_0_1.Load(filepath),
            _ => throw new ArgumentException("Kit version is unsupported.")
        };
    }

    public static void Save(string filepath, Kit kit)
    {
        KitSerializerV_0_1.Save(filepath, kit);
    }
}
