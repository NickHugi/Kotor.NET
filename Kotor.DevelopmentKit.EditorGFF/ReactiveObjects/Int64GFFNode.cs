using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public class Int64GFFNode : BaseFieldGFFNodeViewModel<long>
{
    public override string DisplayType => "Int64";

    public Int64GFFNode(IGFFNode parent, string label, long value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

