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

public class FieldStructGFFNodeViewModel : BaseFieldGFFNodeViewModel<Int32>, IStructGFFTreeNodeViewModel
{
    public override string DisplayType => "Struct";

    public int StructID
    {
        get => FieldValue;
        set => FieldValue = value;
    }

    private ObservableCollection<BaseGFFNodeViewModel> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFNodeViewModel> Children => new(_children);

    bool IStructGFFTreeNodeViewModel.IsDeleted => IsDeleted;


    public FieldStructGFFNodeViewModel(IGFFNodeViewModel parent, string label) : base(parent, label)
    {
    }
    public FieldStructGFFNodeViewModel(IGFFNodeViewModel parent, string label, int structID) : this(parent, label) 
    {
        FieldValue = structID;
    }
    public FieldStructGFFNodeViewModel(IGFFNodeViewModel parent, string label, GFFStruct gffStruct) : this(parent, label)
    {
        //PopulateStruct(gffStruct);
    }

    public T AddField<T>(T field) where T : BaseFieldGFFNodeViewModel
    {
        _children.Add(field);
        Expanded = true;
        return field;
    }

    public void DeleteField(BaseFieldGFFNodeViewModel field)
    {
        _children.Remove(field);
    }

    public BaseFieldGFFNodeViewModel? GetField(string label)
    {
        return (BaseFieldGFFNodeViewModel?)Children.FirstOrDefault(x => x.Label == label);
    }
}

