using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class UInt16GFFTreeNodeViewModel : IFieldGFFTreeNodeViewModel<ushort>
{
    public override string Type => "UInt16";

    public UInt16GFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, ushort value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

