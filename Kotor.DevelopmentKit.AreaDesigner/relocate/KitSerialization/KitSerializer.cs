using System;
using System.IO;
using Newtonsoft.Json;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;

public class KitSerializer
{
    public static Kit Load(string filepath)
    {
        var json = File.ReadAllText(filepath);
        dynamic data = JsonConvert.DeserializeObject(json);
        string format = (string)data.format.Value;

        return format switch
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
