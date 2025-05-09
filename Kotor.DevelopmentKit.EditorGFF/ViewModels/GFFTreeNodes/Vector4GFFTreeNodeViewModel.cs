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

public class Vector4GFFTreeNodeViewModel : ReactiveObject, IFieldGFFTreeNodeViewModel<Vector4ViewModel>
{
    private string _label = "";
    public string Label
    {
        get => _label;
        set => this.RaiseAndSetIfChanged(ref _label, value);
    }

    public bool CanEditLabel => true;

    private Vector4ViewModel _fieldValue;
    public Vector4ViewModel FieldValue
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

    public string Type => "Vector4";
    public string Value => $"{FieldValue.X}, {FieldValue.Y}, {FieldValue.Z}, {FieldValue.W}";

    public Vector4GFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label)
    {
        Parent = parent;
        Label = label;
        _fieldValue = new();

        this.ObservableForProperty(x => x.Label).Subscribe(x => this.RaisePropertyChanged(nameof(Label)));
        this.ObservableForProperty(x => x.FieldValue).Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
        FieldValue.WhenAnyPropertyChanged().Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
    }
    public Vector4GFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, Vector4ViewModel value) : this(parent, label)
    {
        FieldValue = value;
    }
    public Vector4GFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, Vector4 value) : this(parent, label)
    {
        FieldValue = new()
        {
            X = value.X,
            Y = value.Y,
            Z = value.Z,
            W = value.W
        };
    }

    public void Delete()
    {
        ((BaseStructGFFTreeNodeViewModel)Parent).DeleteField(this);
    }
}

