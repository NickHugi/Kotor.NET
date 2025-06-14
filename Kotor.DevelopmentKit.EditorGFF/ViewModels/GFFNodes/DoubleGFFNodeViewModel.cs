using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class DoubleGFFNodeViewModel : BaseFieldGFFNodeViewModel<double>
{
    public override string DisplayType => "Double";

    public DoubleGFFNodeViewModel(IGFFNodeViewModel parent, string label, double value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

