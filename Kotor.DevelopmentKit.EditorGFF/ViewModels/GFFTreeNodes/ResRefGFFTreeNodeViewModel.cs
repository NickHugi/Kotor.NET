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

public class ResRefGFFTreeNodeViewModel : IFieldGFFTreeNodeViewModel<ResRefViewModel>
{
    public override string Type => "ResRef";

    public ResRefGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label) : base(parent, label)
    {
        FieldValue = new();
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
}

