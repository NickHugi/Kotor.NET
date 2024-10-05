using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Kotor.NET.Resources.KotorMDL.Controllers;

namespace Kotor.NET.Resources.KotorMDL.Nodes;

public class MDLNode
{
    public string Name { get; set; }
    public ushort NodeIndex { get; set; }
    public List<MDLNode> Children { get; set; }

    internal List<IMDLControllerRow<BaseMDLControllerData>> _controllerRows { get; }

    public MDLNode(string name)
    {
        Name = name;
        Children = new();
        _controllerRows = new();
    }

    public MDLController<TData> GetController<TData>() where TData : BaseMDLControllerData
    {
        return new MDLController<TData>(_controllerRows);
    }
    public List<MDLController> GetControllers()
    {
        var typesControllerData = _controllerRows.Select(x => x.Data.First().GetType()).ToList();
        var controllers = typesControllerData.Select(x => new MDLController(x, _controllerRows)).ToList();
        return controllers;
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
