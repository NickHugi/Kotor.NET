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

public class FieldListGFFNodeViewModel : IFieldGFFNodeViewModel
{
    public override string Type => "List";
    public override string Value => $"";

    private ObservableCollection<BaseGFFNodeViewModel> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFNodeViewModel> Children => new(_children);


    public FieldListGFFNodeViewModel(IGFFNodeViewModel parent, string label) : base(parent)
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

    public T AddField<T>(T field) where T : IFieldGFFNodeViewModel
    {
        _children.Add(field);
        Expanded = true;
        return field;
    }

    public void DeleteField(IFieldGFFNodeViewModel field)
    {
        _children.Remove(field);
    }

    public IFieldGFFNodeViewModel? GetField(string label)
    {
        return (IFieldGFFNodeViewModel?)Children.FirstOrDefault(x => x.Label == label);
    }

    public override void Delete() => throw new NotImplementedException();
}

