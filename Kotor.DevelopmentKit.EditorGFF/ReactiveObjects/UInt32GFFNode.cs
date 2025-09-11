using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public class UInt32GFFNode : BaseFieldGFFNodeViewModel<uint>
{
    public override string DisplayType => "UInt32";

    public UInt32GFFNode(IGFFNode parent, string label, uint value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

