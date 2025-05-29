using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class Int64GFFTreeNodeViewModel : IFieldGFFTreeNodeViewModel<long>
{
    public override string Type => "Int64";

    public Int64GFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, long value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

