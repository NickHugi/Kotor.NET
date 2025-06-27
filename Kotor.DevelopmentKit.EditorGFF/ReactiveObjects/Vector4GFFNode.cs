using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using Kotor.NET.Common.Data;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public class Vector4GFFNode : BaseFieldGFFNodeViewModel<Vector4ViewModel>
{
    private ReadOnlyObservableCollection<BaseGFFNode> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFNode> Children => _children;

    public override string DisplayType => "Vector4";
    public override string DisplayValue => $"{FieldValue.X}, {FieldValue.Y}, {FieldValue.Z}, {FieldValue.W}";

    public Vector4GFFNode(IGFFNode parent, string label) : base(parent, label)
    {
        FieldValue = new();
        FieldValue.WhenAnyPropertyChanged().Subscribe(x => this.RaisePropertyChanged(nameof(DisplayValue)));
    }
    public Vector4GFFNode(IGFFNode parent, string label, Vector4ViewModel value) : this(parent, label)
    {
        FieldValue = value;
    }
    public Vector4GFFNode(IGFFNode parent, string label, Vector4 value) : this(parent, label)
    {
        FieldValue = new()
        {
            X = value.X,
            Y = value.Y,
            Z = value.Z,
            W = value.W
        };
    }
    public Vector4GFFNode(IGFFNode parent, string label, float x, float y, float z, float w) : this(parent, label)
    {
        FieldValue = new()
        {
            X = x,
            Y = y,
            Z = z,
            W = w
        };
    }
}

