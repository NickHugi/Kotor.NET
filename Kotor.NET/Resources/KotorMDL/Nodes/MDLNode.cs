using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorMDL.Controllers;

namespace Kotor.NET.Resources.KotorMDL.Nodes;

public class MDLNode
{
    public string Name { get; set; }
    public ushort NodeIndex { get; set; }
    public List<MDLController<BaseMDLControllerRow<BaseMDLControllerData>>> Controllers { get; set; }
    public List<MDLNode> Children { get; set; }

    public MDLNode(string name)
    {
        Name = name;
        Controllers = new();
        Children = new();
    }

    public IEnumerable<MDLNode> GetAllAncestors()
    {
        var ancestors = Children.ToList();
        return ancestors.Concat(ancestors.SelectMany(x => x.GetAllAncestors()));
    }

    public override string ToString()
    {
        return $"MDLNode '{Name}'";
    }
}
