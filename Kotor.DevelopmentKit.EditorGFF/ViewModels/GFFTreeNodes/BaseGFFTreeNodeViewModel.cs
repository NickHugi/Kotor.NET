using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public interface IGFFTreeNodeViewModel
{
    public string Label { get; set; }
    public bool CanEditLabel { get; }

    public string Type { get; }
    public string Value { get; }

    public IGFFTreeNodeViewModel? Parent { get; }
    public ReadOnlyObservableCollection<BaseGFFTreeNodeViewModel> Children { get; }

    //public void Delete();
}

public abstract class BaseGFFTreeNodeViewModel : ReactiveObject, IGFFTreeNodeViewModel
{
    public abstract string Label { get; set; }
    public abstract bool CanEditLabel { get; }

    public abstract string Type { get; }
    public abstract string Value { get; }

    private bool _expanded = true;
    public bool Expanded
    {
        get => _expanded;
        set => this.RaiseAndSetIfChanged(ref _expanded, value);
    }

    public int Version { get; set; }
    public int SavedVersion { get; set; }

    public IGFFTreeNodeViewModel? Parent { get; }
    public abstract ReadOnlyObservableCollection<BaseGFFTreeNodeViewModel> Children { get; }


    public BaseGFFTreeNodeViewModel(IGFFTreeNodeViewModel? parent)
    {
        Parent = parent;
    }

    public abstract void Delete();
}
