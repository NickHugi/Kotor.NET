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

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class StructGFFTreeNodeViewModel : IFieldGFFTreeNodeViewModel<Int32>, BaseStructGFFTreeNodeViewModel
{
    public override string Type => "Struct";

    public int StructID
    {
        get => FieldValue;
        set => FieldValue = value;
    }

    private ObservableCollection<BaseGFFTreeNodeViewModel> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFTreeNodeViewModel> Children => new(_children);


    public StructGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label) : base(parent, label)
    {
    }
    public StructGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, int structID) : this(parent, label) 
    {
        FieldValue = structID;
    }
    public StructGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, GFFStruct gffStruct) : this(parent, label)
    {
        //PopulateStruct(gffStruct);
    }

    public T AddField<T>(T field) where T : IFieldGFFTreeNodeViewModel
    {
        _children.Add(field);
        Expanded = true;
        return field;
    }

    public void DeleteField(IFieldGFFTreeNodeViewModel field)
    {
        _children.Remove(field);
    }

    public IFieldGFFTreeNodeViewModel? GetField(string label)
    {
        return (IFieldGFFTreeNodeViewModel?)Children.FirstOrDefault(x => x.Label == label);
    }
}

