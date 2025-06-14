using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class StringGFFNodeViewModel : BaseFieldGFFNodeViewModel<string>
{
    public override string DisplayType => "String";

    public StringGFFNodeViewModel(IGFFNodeViewModel parent, string label, string value = "") : base(parent, label)
    {
        FieldValue = value;
    }
}

