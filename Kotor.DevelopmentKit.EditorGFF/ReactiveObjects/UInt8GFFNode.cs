using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public class UInt8GFFNode : BaseFieldGFFNodeViewModel<byte>
{
    public override string DisplayType => "UInt8";

    public UInt8GFFNode(IGFFNode parent, string label, byte value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

