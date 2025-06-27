using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public class Int16GFFNode : BaseFieldGFFNodeViewModel<short>
{
    public override string DisplayType => "Int16";

    public Int16GFFNode(IGFFNode parent, string label, short value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

