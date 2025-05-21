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

public class ListGFFTreeNodeViewModel : ReactiveObject, IFieldGFFTreeNodeViewModel
{
    private string _label = "";
    public string Label
    {
        get => _label;
        set => this.RaiseAndSetIfChanged(ref _label, value);
    }

    public bool CanEditLabel => true;

    private bool _expanded;
    public bool Expanded
    {
        get => _expanded;
        set => this.RaiseAndSetIfChanged(ref _expanded, value);
    }

    public IGFFTreeNodeViewModel Parent { get; }

    private ObservableCollection<IGFFTreeNodeViewModel> _children = new([]);
    public ReadOnlyObservableCollection<IGFFTreeNodeViewModel> Children => new(_children);

    public string Type => "List";
    public string Value => $"";

    public ListGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label)
    {
        Parent = parent;
        Label = label;

        this.ObservableForProperty(x => x.Label).Subscribe(x => this.RaisePropertyChanged(nameof(Label)));
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
        _children.Remove(@struct);
    }

    public void Delete()
    {
        ((BaseStructGFFTreeNodeViewModel)Parent).DeleteField(this);
    }
}

