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

public class Vector3GFFNodeViewModel : BaseFieldGFFNodeViewModel<Vector3ViewModel>
{
    public override string DisplayType => "Vector3";
    public override string DisplayValue => $"{FieldValue.X}, {FieldValue.Y}, {FieldValue.Z}";

    public Vector3GFFNodeViewModel(IGFFNodeViewModel parent, string label) : base(parent, label)
    {
        FieldValue = new();
        FieldValue.WhenAnyPropertyChanged().Subscribe(x => this.RaisePropertyChanged(nameof(DisplayValue)));
    }
    public Vector3GFFNodeViewModel(IGFFNodeViewModel parent, string label, Vector3ViewModel value) : this(parent, label)
    {
        FieldValue = value;
    }
    public Vector3GFFNodeViewModel(IGFFNodeViewModel parent, string label, Vector3 value) : this(parent, label)
    {
        FieldValue = new()
        {
            X = value.X,
            Y = value.Y,
            Z = value.Z
        };
    }
    public Vector3GFFNodeViewModel(IGFFNodeViewModel parent, string label, float x, float y, float z) : this(parent, label)
    {
        FieldValue = new()
        {
            X = x,
            Y = y,
            Z = z
        };
    }
}

