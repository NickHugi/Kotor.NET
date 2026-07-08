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

    public TileTemplate Tile(string templateID) => Objects.OfType<TileTemplate>().Single(x => x.TemplateID == templateID);
    public WallTemplate Wall(string templateID) => Objects.OfType<WallTemplate>().Single(x => x.TemplateID == templateID);
    public DoorFrameTemplate DoorFrame(string templateID) => Objects.OfType<DoorFrameTemplate>().Single(x => x.TemplateID == templateID);
    public CeilingTemplate Ceiling(string templateID) => Objects.OfType<CeilingTemplate>().Single(x => x.TemplateID == templateID);
    public InnerCornerTemplate InnerCorner(string templateID) => Objects.OfType<InnerCornerTemplate>().Single(x => x.TemplateID == templateID);
    public OuterCornerTemplate OuterCorner(string templateID) => Objects.OfType<OuterCornerTemplate>().Single(x => x.TemplateID == templateID);
    public UltimateWorldObjectTemplate Object(string templateID) => Objects/*.Where(x => x.GetType() == typeof(UltimateWorldObjectTemplate))*/.Single(x => x.TemplateID == templateID);

    public Kit(string filepath, string kitID, int version, string name)
    {
        FilePath = filepath;
        KitID = kitID;
        Name = name;
        Version = version;
    }   
}
