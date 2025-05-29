using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class UInt64GFFTreeNodeViewModel : IFieldGFFTreeNodeViewModel<ulong>
{
    public override string Type => "UInt64";

    public UInt64GFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, ulong value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

