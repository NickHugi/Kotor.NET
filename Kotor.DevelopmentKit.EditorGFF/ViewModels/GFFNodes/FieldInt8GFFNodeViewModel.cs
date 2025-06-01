using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class FieldInt8GFFNodeViewModel : BaseFieldGFFTreeNodeViewModel<sbyte>
{
    public override string Type => "Int8";

    public FieldInt8GFFNodeViewModel(IGFFNodeViewModel parent, string label, sbyte value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

