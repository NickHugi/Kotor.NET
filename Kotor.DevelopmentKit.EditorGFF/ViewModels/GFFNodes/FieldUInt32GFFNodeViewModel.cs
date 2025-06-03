using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class FieldUInt32GFFNodeViewModel : BaseFieldGFFTreeNodeViewModel<uint>
{
    public override string DisplayType => "UInt32";

    public FieldUInt32GFFNodeViewModel(IGFFNodeViewModel parent, string label, uint value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

