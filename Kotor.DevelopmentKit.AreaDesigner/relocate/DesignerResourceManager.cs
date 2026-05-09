using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Kotor.NET.Resources.KotorTPC;
using Kotor.NET.Resources.KotorTPC.TextureFormats;
using Kotor.NET.Tools;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

public class DesignerResourceManager
{
    public string KitDirectory { get; set; }

    private List<string> _files;

    public DesignerResourceManager(string kitDirectory)
    {
        KitDirectory = kitDirectory;
        _files = Directory.GetFiles(kitDirectory, "*.*", SearchOption.AllDirectories).ToList();
    }

    public TPC GetTexture(string texture)
    {
        var file = _files.FirstOrDefault(x =>
            Path.GetExtension(x).Equals(".tpc", StringComparison.OrdinalIgnoreCase)
            && Path.GetFileName(x).Equals(texture, StringComparison.OrdinalIgnoreCase));

        if (file is null)
        {
            return new TPC(64, 64, 1, 1, TPCTextureFormat.RGB);
        }
        else
        {
            return TPC.FromFile(file);
        }
    }
}
