using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class FieldUInt64GFFNodeViewModel : BaseFieldGFFNodeViewModel<ulong>
{
    public override string DisplayType => "UInt64";

    public FieldUInt64GFFNodeViewModel(IGFFNodeViewModel parent, string label, ulong value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

