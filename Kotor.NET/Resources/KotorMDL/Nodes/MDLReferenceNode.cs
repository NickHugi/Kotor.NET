using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Nodes;

public class MDLReferenceNode : MDLNode
{
    public string ModelResRef { get; set; }

    public MDLReferenceNode(string name) : base(name)
    {
    }

    public override string ToString()
    {
        return $"MDLReferenceNode '{Name}'";
    }
}
