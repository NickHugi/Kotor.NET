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

public class FieldListGFFNodeViewModel : BaseFieldGFFNodeViewModel
{
    public override string DisplayType => "List";
    public override string DisplayValue => $"";

    private ObservableCollection<BaseGFFNodeViewModel> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFNodeViewModel> Children => new(_children);


    public FieldListGFFNodeViewModel(IGFFNodeViewModel parent, string label) : base(parent)
    {
        Label = label;
    }
    public FieldListGFFNodeViewModel(IGFFNodeViewModel parent, string label, GFFList gffList) : this(parent, label)
    {
        Label = label;
    }

    public ListStructGFFNodeViewModel AddStruct()
    {
        var childStruct = new ListStructGFFNodeViewModel(this);
        _children.Add(childStruct);
        Expanded = true;
        return childStruct;
    }
    public ListStructGFFNodeViewModel AddStruct(ListStructGFFNodeViewModel childStruct)
    {
        _children.Add(childStruct);
        Expanded = true;
        return childStruct;
    }

    public void DeleteStruct(IStructGFFTreeNodeViewModel @struct)
    {
        _children.Remove(@struct as BaseGFFNodeViewModel);
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

