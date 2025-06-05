using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class FieldUInt16GFFNodeViewModel : BaseFieldGFFNodeViewModel<ushort>
{
    public override string DisplayType => "UInt16";

    public FieldUInt16GFFNodeViewModel(IGFFNodeViewModel parent, string label, ushort value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

