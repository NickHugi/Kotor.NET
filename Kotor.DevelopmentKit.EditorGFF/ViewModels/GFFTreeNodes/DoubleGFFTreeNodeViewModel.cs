using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class DoubleGFFTreeNodeViewModel : IFieldGFFTreeNodeViewModel<double>
{
    public override string Type => "Double";

    public DoubleGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, double value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

