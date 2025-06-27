using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public class Int8GFFNode : BaseFieldGFFNodeViewModel<sbyte>
{
    public override string DisplayType => "Int8";

    public Int8GFFNode(IGFFNode parent, string label, sbyte value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

