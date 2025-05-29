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

public class Vector3GFFTreeNodeViewModel : IFieldGFFTreeNodeViewModel<Vector3ViewModel>
{
    public override string Type => "Vector3";
    public override string Value => $"{FieldValue.X}, {FieldValue.Y}, {FieldValue.Z}";

    public Vector3GFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label) : base(parent, label)
    {
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
}

