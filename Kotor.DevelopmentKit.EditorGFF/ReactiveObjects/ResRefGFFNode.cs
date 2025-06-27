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

public class ResRefGFFNode : BaseFieldGFFNodeViewModel<ReactiveResRef>
{
    public override string DisplayType => "ResRef";

    public ResRefGFFNode(IGFFNode parent, string label) : base(parent, label)
    {
        FieldValue = new();
        FieldValue.WhenAnyPropertyChanged().Subscribe(x => this.RaisePropertyChanged(nameof(DisplayValue)));
    }
    public ResRefGFFNode(IGFFNode parent, string label, ReactiveResRef value) : this(parent, label)
    {
        FieldValue = value;
    }
    public ResRefGFFNode(IGFFNode parent, string label, ResRef value) : this(parent, label)
    {
        FieldValue = new() { Value = value.Get() }; 
    }
}

