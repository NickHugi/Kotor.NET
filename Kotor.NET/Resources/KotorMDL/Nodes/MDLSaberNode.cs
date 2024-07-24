using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Nodes;

public class MDLSaberNode : MDLTrimeshNode
{
    public int Unknown1 { get; set; }
    public int Unknown2 { get; set; }

    public MDLSaberNode(string name) : base(name)
    {
        AllVertices().ToList().ForEach(x => x._saber = new());
    }

    public override string ToString()
    {
        return $"MDLSaberNode '{Name}'";
    }
}
