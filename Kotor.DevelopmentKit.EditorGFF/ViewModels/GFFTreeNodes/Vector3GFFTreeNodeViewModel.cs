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

public class Vector3GFFTreeNodeViewModel : ReactiveObject, IFieldGFFTreeNodeViewModel<Vector3ViewModel>
{
    private string _label = "";
    public string Label
    {
        get => _label;
        set => this.RaiseAndSetIfChanged(ref _label, value);
    }

    public bool CanEditLabel => true;

    private Vector3ViewModel _fieldValue;
    public Vector3ViewModel FieldValue
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

    public string Type => "Vector3";
    public string Value => $"{FieldValue.X}, {FieldValue.Y}, {FieldValue.Z}";

    public Vector3GFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label)
    {
        Parent = parent;
        Label = label;
        _fieldValue = new();

        this.ObservableForProperty(x => x.Label).Subscribe(x => this.RaisePropertyChanged(nameof(Label)));
        this.ObservableForProperty(x => x.FieldValue).Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
        FieldValue.WhenAnyPropertyChanged().Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
    }
    public Vector3GFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, Vector3ViewModel value) : this(parent, label)
    {
        FieldValue = value;
    }
    public Vector3GFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, Vector3 value) : this(parent, label)
    {
        FieldValue = new()
        {
            X = value.X,
            Y = value.Y,
            Z = value.Z
        };
    }

    public void Delete()
    {
        ((BaseStructGFFTreeNodeViewModel)Parent).DeleteField(this);
    }
}

