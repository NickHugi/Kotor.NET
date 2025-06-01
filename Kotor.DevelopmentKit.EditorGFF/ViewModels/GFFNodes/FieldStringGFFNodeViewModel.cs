using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class FieldStringGFFNodeViewModel : IFieldGFFTreeNodeViewModel<string>
{
    public override string Type => "String";

    public FieldStringGFFNodeViewModel(IGFFNodeViewModel parent, string label, string value = "") : base(parent, label)
    {
        FieldValue = value;
    }
}

