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

public class FieldStructGFFNode : BaseFieldGFFNodeViewModel<GFFStructID>, IStructGFFNode
{
    public override string DisplayType => "Struct";

    public GFFStructID StructID
    {
        get => FieldValue;
        set => FieldValue = value;
    }

    private ObservableCollection<BaseGFFNode> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFNode> Children => new(_children);

    bool IStructGFFNode.IsDeleted => IsDeleted;


    public FieldStructGFFNode(IGFFNode parent, string label) : base(parent, label)
    {
    }
    public FieldStructGFFNode(IGFFNode parent, string label, int structID) : this(parent, label) 
    {
        FieldValue = structID;
    }
    public FieldStructGFFNode(IGFFNode parent, string label, GFFStruct gffStruct) : this(parent, label)
    {
        //PopulateStruct(gffStruct);
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

