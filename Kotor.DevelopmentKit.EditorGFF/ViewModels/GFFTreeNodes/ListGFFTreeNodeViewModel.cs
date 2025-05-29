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

public class ListGFFTreeNodeViewModel : IFieldGFFTreeNodeViewModel
{
    public override string Type => "List";
    public override string Value => $"";

    private ObservableCollection<BaseGFFTreeNodeViewModel> _children = new([]);


    public ListGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label) : base(parent)
    {
        Label = label;
    }

    public StructInListGFFTreeNodeViewModel AddStruct()
    {
        var childStruct = new StructInListGFFTreeNodeViewModel(this);
        _children.Add(childStruct);
        Expanded = true;
        return childStruct;
    }
    public StructInListGFFTreeNodeViewModel AddStruct(StructInListGFFTreeNodeViewModel childStruct)
    {
        _children.Add(childStruct);
        Expanded = true;
        return childStruct;
    }

    public void DeleteStruct(BaseStructGFFTreeNodeViewModel @struct)
    {
        _children.Remove(@struct as BaseGFFTreeNodeViewModel);
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

    public override void Delete() => throw new NotImplementedException();
}

