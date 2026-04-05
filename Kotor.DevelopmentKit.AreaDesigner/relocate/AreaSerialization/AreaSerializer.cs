using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;
using Newtonsoft.Json;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaSerialization;

public class AreaSerializer
{
    public static Area Load(string filepath)
    {
        var json = File.ReadAllText(filepath);
        dynamic data = JsonConvert.DeserializeObject(json);
        string format = (string)data.format.Value;

        return format switch
        {
            "0.1" => AreaSerializer_V0_1.Load(filepath),
            _ => throw new ArgumentException("Kit version is unsupported.")
        };
    }

    public static void Save(string filepath, Area area)
    {
        AreaSerializer_V0_1.Save(filepath, area);
    }
}
