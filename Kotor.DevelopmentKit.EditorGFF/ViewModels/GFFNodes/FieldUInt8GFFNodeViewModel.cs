using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class FieldUInt8GFFNodeViewModel : BaseFieldGFFTreeNodeViewModel<byte>
{
    public override string Type => "UInt8";

    public FieldUInt8GFFNodeViewModel(IGFFNodeViewModel parent, string label, byte value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

