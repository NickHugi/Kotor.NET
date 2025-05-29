using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class Int32GFFTreeNodeViewModel : IFieldGFFTreeNodeViewModel<int>
{
    public override string Type => "Int32";

    public Int32GFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, int value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

