using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public abstract class BaseStructGFFTreeNodeViewModel : ReactiveObject, IGFFTreeNodeViewModel
{
    public abstract string Label { get; set; }
    public abstract bool CanEditLabel { get; }

    public int _structID;
    public int StructID
    {
        get => _structID;
        set => this.RaiseAndSetIfChanged(ref _structID, value);
    }
    
    private bool _expanded;
    public bool Expanded
    {
        get => _expanded;
        set => this.RaiseAndSetIfChanged(ref _expanded, value);
    }

    public string Type => "Struct";
    public string Value => StructID.ToString();
    
    public IGFFTreeNodeViewModel? Parent { get; }

    private ObservableCollection<IGFFTreeNodeViewModel> _children = new([]);
    public ReadOnlyObservableCollection<IGFFTreeNodeViewModel> Children => new(_children);

    public BaseStructGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent)
    {
        Parent = parent;
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

    public abstract void Delete();
}
