using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorMDL.Nodes;

namespace Kotor.NET.Resources.KotorMDL;

public class MDLDanglyNode : MDLTrimeshNode
{
    public float Displacement { get; set; }
    public float Tightness { get; set; }
    public float Period { get; set; }

    public MDLDanglyNode(string name) : base(name)
    {
        AllVertices().ToList().ForEach(x => x._dangly = new());
    }

    public override string ToString()
    {
        return $"MDLDanglyNode '{Name}'";
    }
}
