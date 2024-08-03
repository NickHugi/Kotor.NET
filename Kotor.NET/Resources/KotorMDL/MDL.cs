using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorMDL.Nodes;

namespace Kotor.NET.Resources.KotorMDL;

public class MDL
{
    public List<MDLAnimation> Animations = new();

    public MDL? Supermodel { get; set; }
    public MDLNode Root { get; set; } = new MDLNode("root");

    public byte ModelType { get; set; }
    public bool AffectedByFog { get; set; }
    public float AnimationScale { get; set; } = 1;
    public string SupermodelName { get; set; } = "NULL";
    public string Name { get; set; } = "model";

    public BoundingBox BoundingBox { get; set; } = new();
    public float Radius { get; set; }

    public void RecalculateBounds()
    {
        // Iterate through nodes to determine new BoundingBox and Radius values.
        // If I recall correctly KotOR tends to pad out the values a small amount (~0.1?)
        throw new NotImplementedException();
    }
}
