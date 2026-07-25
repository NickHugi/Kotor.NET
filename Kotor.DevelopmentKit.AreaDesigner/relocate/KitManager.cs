using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

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
    public Kit? Get(string kitID) => Kits.SingleOrDefault(x => x.KitID == kitID);

    public IEnumerable<WorldObjectTemplate> AllTemplates(string classID)
    {
        return Kits.SelectMany(x => x.Objects).Where(x => x.ClassID == classID);
    }
}
