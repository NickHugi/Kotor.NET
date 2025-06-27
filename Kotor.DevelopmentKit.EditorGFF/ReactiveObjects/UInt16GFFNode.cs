using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public class UInt16GFFNode : BaseFieldGFFNodeViewModel<ushort>
{
    public override string DisplayType => "UInt16";

    public UInt16GFFNode(IGFFNode parent, string label, ushort value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

