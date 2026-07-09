using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

public class Kit
{
    public static KitManager Manager { get; } = new();

    public string KitID { get; }
    public string Name { get; }
    public string FilePath { get; }
    public int Version { get; }
    public ICollection<UltimateWorldObjectTemplate> Objects { get; init; } = [];

    public UltimateWorldObjectTemplate Object(string templateID) => Objects.Single(x => x.TemplateID == templateID);

    public Kit(string filepath, string kitID, int version, string name)
    {
        FilePath = filepath;
        KitID = kitID;
        Name = name;
        Version = version;
    }   
}
