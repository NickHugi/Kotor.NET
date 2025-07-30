using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public class SingleGFFNode : BaseFieldGFFNodeViewModel<float>
{
    public override string DisplayType => "Single";

    public SingleGFFNode(IGFFNode parent, string label, float value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

