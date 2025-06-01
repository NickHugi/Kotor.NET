using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public abstract class BaseGFFNodeViewModel : ReactiveObject, IGFFNodeViewModel
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

    public IGFFNodeViewModel? Parent { get; }
    public abstract ReadOnlyObservableCollection<BaseGFFNodeViewModel> Children { get; }


    public BaseGFFNodeViewModel(IGFFNodeViewModel? parent)
    {
        Parent = parent;
    }

    public abstract void Delete();
}
