using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public class DoubleGFFNode : BaseFieldGFFNodeViewModel<double>
{
    public override string DisplayType => "Double";

    public DoubleGFFNode(IGFFNode parent, string label, double value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

