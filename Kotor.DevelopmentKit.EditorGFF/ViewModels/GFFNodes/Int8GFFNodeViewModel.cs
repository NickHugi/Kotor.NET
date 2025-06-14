using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class Int8GFFNodeViewModel : BaseFieldGFFNodeViewModel<sbyte>
{
    public override string DisplayType => "Int8";

    public Int8GFFNodeViewModel(IGFFNodeViewModel parent, string label, sbyte value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

