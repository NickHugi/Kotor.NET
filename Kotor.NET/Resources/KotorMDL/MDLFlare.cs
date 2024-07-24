using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorMDL;

public class MDLFlare
{
    public float Size { get; set; }
    public float Position { get; set; }
    public Vector3 ColourShift { get; set; } = new();
    public string TextureName { get; set; } = "";
}
