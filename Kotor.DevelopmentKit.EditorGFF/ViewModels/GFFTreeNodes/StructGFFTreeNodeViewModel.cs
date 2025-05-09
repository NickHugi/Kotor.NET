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

public class StructGFFTreeNodeViewModel : ReactiveObject, IFieldGFFTreeNodeViewModel, IStructGFFTreeNodeViewModel
{
    private string _label = "";
    public string Label
    {
        get => _label;
        set => this.RaiseAndSetIfChanged(ref _label, value);
    }

    public int _structID;
    public int StructID
    {
        get => _structID;
        set => this.RaiseAndSetIfChanged(ref _structID, value);
    }

    public bool CanEditLabel => true;

    private bool _expanded;
    public bool Expanded
    {
        get => _expanded;
        set => this.RaiseAndSetIfChanged(ref _expanded, value);
    }

    public IGFFTreeNodeViewModel Parent { get; }

    private ObservableCollection<IGFFTreeNodeViewModel> _children = new();
    public ReadOnlyObservableCollection<IGFFTreeNodeViewModel> Children => new(_children);

    public string Type => "Struct";
    public string Value => $"";

    public StructGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label)
    {
        Parent = parent;
        Label = label;

        this.ObservableForProperty(x => x.Label).Subscribe(x => this.RaisePropertyChanged(nameof(Label)));
    }

    public StructGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, int structID) : this(parent, label) 
    {
        StructID = structID;
    }

    public void AddField(IFieldGFFTreeNodeViewModel field)
    {
        _children.Add(field);
        Expanded = true;
    }
    public void DeleteField(IFieldGFFTreeNodeViewModel field)
    {
        _children.Remove(field);
    }

    public void Delete()
    {
        ((IStructGFFTreeNodeViewModel)Parent).DeleteField(this);
    }
}

