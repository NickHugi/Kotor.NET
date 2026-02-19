using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Nodes;

public class MDLWalkmeshNode : MDLTrimeshNode
{
    public MDLWalkmeshAABBNode? RootNode { get; set; }

    public MDLWalkmeshNode(string name) : base(name)
    {
    }
}
