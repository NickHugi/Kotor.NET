using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.NET.Common.Data;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class ResRefGFFTreeNodeViewModel : ReactiveObject, IFieldGFFTreeNodeViewModel<ResRefViewModel>
{
    private string _label = "";
    public string Label
    {
        get => _label;
        set => this.RaiseAndSetIfChanged(ref _label, value);
    }

    public bool CanEditLabel => true;

    private ResRefViewModel _fieldValue;
    public ResRefViewModel FieldValue
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

    public string Type => "ResRef";
    public string Value => FieldValue.Value.ToString();

    public ResRefGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label)
    {
        Parent = parent;
        Label = label;
        _fieldValue = new();

        this.ObservableForProperty(x => x.Label).Subscribe(x => this.RaisePropertyChanged(nameof(Label)));
        this.ObservableForProperty(x => x.FieldValue).Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
        FieldValue.WhenAnyPropertyChanged().Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
    }
    public ResRefGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, ResRefViewModel value) : this(parent, label)
    {
        FieldValue = value;
    }
    public ResRefGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, ResRef value) : this(parent, label)
    {
        FieldValue = new() { Value = value.Get() }; 
    }

    public void Delete()
    {
        ((IStructGFFTreeNodeViewModel)Parent).DeleteField(this);
    }
}

