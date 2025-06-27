using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public class Int32GFFNode : BaseFieldGFFNodeViewModel<int>
{
    public override string DisplayType => "Int32";

    public Int32GFFNode(IGFFNode parent, string label, int value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

