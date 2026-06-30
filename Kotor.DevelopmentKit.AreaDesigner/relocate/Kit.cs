using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

public class Kit
{
    public static KitManager Manager { get; } = new();

    public string ID { get; }
    public string Name { get; }
    public string FilePath { get; }
    public int Version { get; }
    public ICollection<WorldObjectTemplate> Objects { get; init; } = [];

    // TODO - and kit
    public FloorTemplate Floor(string id) => Objects.OfType<FloorTemplate>().Single(x => x.TemplateID == id);
    public TileTemplate Tile(string id) => Objects.OfType<TileTemplate>().Single(x => x.TemplateID == id);
    public WallTemplate Wall(string id) => Objects.OfType<WallTemplate>().Single(x => x.TemplateID == id);
    public DoorFrameTemplate DoorFrame(string id) => Objects.OfType<DoorFrameTemplate>().Single(x => x.TemplateID == id);
    public CeilingTemplate Ceiling(string id) => Objects.OfType<CeilingTemplate>().Single(x => x.TemplateID == id);
    public InnerCornerTemplate InnerCorner(string id) => Objects.OfType<InnerCornerTemplate>().Single(x => x.TemplateID == id);
    public OuterCornerTemplate OuterCorner(string id) => Objects.OfType<OuterCornerTemplate>().Single(x => x.TemplateID == id);
    public WorldObjectTemplate Object(string id) => Objects.Where(x => x.GetType() == typeof(WorldObjectTemplate)).Single(x => x.TemplateID == id);

    public Kit(string filepath, string id, int version, string name)
    {
        FilePath = filepath;
        ID = id;
        Name = name;
        Version = version;
    }   
}

public class KitManager
{
    public string ActiveDirectory = @"C:/Kits";
    public ICollection<Kit> Kits { get; } = [];

    public void Refresh()
    {
        Kits.Clear();
        Directory.GetFiles(Kit.Manager.ActiveDirectory)
            .Where(x => Path.GetExtension(x).ToLower() == ".kit")
            .Select(KitSerializer.Load)
            .ToList()
            .ForEach(Kits.Add);
    }
    public Kit Get(string id) => Kits.Single(x => x.ID == id);
}
