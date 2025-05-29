using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class Int16GFFTreeNodeViewModel : IFieldGFFTreeNodeViewModel<short>
{
    public override string Type => "Int16";

    public Int16GFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, short value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

