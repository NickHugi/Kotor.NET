using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Kotor.NET.Formats.BinaryGFF;
using Kotor.NET.Resources.KotorGFF;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public class ListStructGFFNode : BaseGFFNode, IStructGFFNode
{
    public override string Label
    {
        get => $"[{(Parent as BaseGFFNode)!.Children.IndexOf(this).ToString()}]";
        set => throw new NotImplementedException();
    }
    public override bool CanEditLabel => false;
    public override string DisplayType => "Struct";
    public override string DisplayValue => StructID.ToString();

    private int _structID;
    public int StructID
    {
        get => _structID;
        set => this.RaiseAndSetIfChanged(ref _structID, value);
    }

    private ObservableCollection<BaseGFFNode> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFNode> Children => new(_children);

    bool IStructGFFNode.IsDeleted => IsDeleted;

    public ListStructGFFNode(IGFFNode parent) : base(parent)
    {
    }
    public ListStructGFFNode(IGFFNode parent, GFFStruct gffStruct) : base(parent)
    {
        this.PopulateStruct(gffStruct);
    }
    public ListStructGFFNode(IGFFNode parent, int structID) : base(parent)
    {
        StructID = structID;
    }

    public override void Delete()
    {
        if (Parent is ListGFFNode listNode)
        {
            listNode.DeleteStruct(this);
        }
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
