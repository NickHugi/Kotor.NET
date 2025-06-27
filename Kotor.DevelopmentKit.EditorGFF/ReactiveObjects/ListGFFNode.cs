using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public class ListGFFNode : BaseFieldGFFNode
{
    public override string DisplayType => "List";
    public override string DisplayValue => $"";

    private ObservableCollection<BaseGFFNode> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFNode> Children => new(_children);


    public ListGFFNode(IGFFNode parent, string label) : base(parent)
    {
        Label = label;
    }
    public ListGFFNode(IGFFNode parent, string label, GFFList gffList) : this(parent, label)
    {
        Label = label;
    }

    public ListStructGFFNode AddStruct()
    {
        var childStruct = new ListStructGFFNode(this);
        _children.Add(childStruct);
        Expanded = true;
        return childStruct;
    }
    public ListStructGFFNode AddStruct(ListStructGFFNode childStruct)
    {
        _children.Add(childStruct);
        Expanded = true;
        return childStruct;
    }

    public void DeleteStruct(IStructGFFNode @struct)
    {
        _children.Remove(@struct as BaseGFFNode);
    }

    public T AddField<T>(T field) where T : BaseFieldGFFNode
    {
        _children.Add(field);
        Expanded = true;
        return field;
    }

    public void DeleteField(BaseFieldGFFNode field)
    {
        _children.Remove(field);
    }

    public BaseFieldGFFNode? GetField(string label)
    {
        return (BaseFieldGFFNode?)Children.FirstOrDefault(x => x.Label == label);
    }
}

