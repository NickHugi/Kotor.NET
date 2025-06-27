using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Kotor.NET.Resources.KotorGFF;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public class RootStructGFFNode : BaseGFFNode, IStructGFFNode
{
    private int _structID;
    public int StructID
    {
        get => _structID;
        set => this.RaiseAndSetIfChanged(ref _structID, value);
    }

    public override string Label
    {
        get => "[Root]";
        set => throw new NotSupportedException();
    }
    public override bool CanEditLabel => false;
    public override string DisplayType => "Struct";
    public override string DisplayValue => StructID.ToString();

    private ObservableCollection<BaseGFFNode> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFNode> Children => new(_children);


    public RootStructGFFNode() : base(null)
    {
    }
    public RootStructGFFNode(GFFStruct gffStruct) : base(null)
    {
    }

    public override void Delete()
    {
        throw new InvalidOperationException("Cannot delete the root node of a GFF resource.");
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
