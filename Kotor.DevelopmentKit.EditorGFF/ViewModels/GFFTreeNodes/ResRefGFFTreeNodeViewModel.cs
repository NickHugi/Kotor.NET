using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Kotor.NET.Common.Data;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class ResRefGFFTreeNodeViewModel : ReactiveObject, IFieldGFFTreeNodeViewModel<ResRef>
{
    private string _label = "";
    public string Label
    {
        get => _label;
        set => this.RaiseAndSetIfChanged(ref _label, value);
    }

    private ResRef _fieldValue;
    public ResRef FieldValue
    {
        get => _fieldValue;
        set => this.RaiseAndSetIfChanged(ref _fieldValue, value);
    }

    private bool _expanded;
    public bool Expanded
    {
        get => _expanded;
        set => this.RaiseAndSetIfChanged(ref _expanded, value);
    }

    public IGFFTreeNodeViewModel Parent { get; }

    private ReadOnlyObservableCollection<IGFFTreeNodeViewModel> _children = new([]);
    public ReadOnlyObservableCollection<IGFFTreeNodeViewModel> Children => _children;

    public string Name => Label;
    public string Type => "ResRef";
    public string Value => FieldValue.ToString();

    public ResRefGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label)
    {
        Parent = parent;
        Label = label;
        _fieldValue = "";

        this.ObservableForProperty(x => x.Label).Subscribe(x => this.RaisePropertyChanged(nameof(Name)));
        this.ObservableForProperty(x => x.FieldValue).Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
    }
    public ResRefGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, ResRef value) : this(parent, label)
    {
        FieldValue = value;
    }

    public void Delete()
    {
        ((IStructGFFTreeNodeViewModel)Parent).DeleteField(this);
    }
}

