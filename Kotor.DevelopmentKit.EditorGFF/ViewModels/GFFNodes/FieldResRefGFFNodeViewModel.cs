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

public class FieldResRefGFFNodeViewModel : BaseFieldGFFNodeViewModel<ResRefViewModel>
{
    public override string DisplayType => "ResRef";

    public FieldResRefGFFNodeViewModel(IGFFNodeViewModel parent, string label) : base(parent, label)
    {
        FieldValue = new();
        FieldValue.WhenAnyPropertyChanged().Subscribe(x => this.RaisePropertyChanged(nameof(DisplayValue)));
    }
    public FieldResRefGFFNodeViewModel(IGFFNodeViewModel parent, string label, ResRefViewModel value) : this(parent, label)
    {
        FieldValue = value;
    }
    public FieldResRefGFFNodeViewModel(IGFFNodeViewModel parent, string label, ResRef value) : this(parent, label)
    {
        FieldValue = new() { Value = value.Get() }; 
    }
}

