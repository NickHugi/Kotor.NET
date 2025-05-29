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

public class Vector4GFFTreeNodeViewModel : IFieldGFFTreeNodeViewModel<Vector4ViewModel>
{
    private ReadOnlyObservableCollection<BaseGFFTreeNodeViewModel> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFTreeNodeViewModel> Children => _children;

    public override string Type => "Vector4";
    public override string Value => $"{FieldValue.X}, {FieldValue.Y}, {FieldValue.Z}, {FieldValue.W}";

    public Vector4GFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label) : base(parent, label)
    {
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
}

