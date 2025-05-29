using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class UInt32GFFTreeNodeViewModel : IFieldGFFTreeNodeViewModel<uint>
{
    public override string Type => "UInt32";

    public UInt32GFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, uint value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

