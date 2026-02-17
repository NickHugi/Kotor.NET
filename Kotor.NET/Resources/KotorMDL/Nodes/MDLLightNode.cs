using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Nodes;

public class MDLLightNode : MDLNode
{
    public float FlareRadius { get; set; }
    public uint LightPriority { get; set; }
    public uint AmbientOnly { get; set; }
    public uint DynamicType { get; set; }
    public uint AffectDynamic { get; set; }
    public uint Shadow { get; set; }
    public uint Flare { get; set; }
    public uint FadingLight { get; set; }
    public List<MDLFlare> Flares { get; set; } = new();

    public MDLLightNode(string name) : base(name)
    {
    }

    public override string ToString()
    {
        return $"MDLLightNode '{Name}'";
    }
}
