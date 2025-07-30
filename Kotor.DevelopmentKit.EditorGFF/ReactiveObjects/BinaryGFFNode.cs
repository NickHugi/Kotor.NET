using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public class BinaryGFFNode : BaseFieldGFFNodeViewModel<byte[]>
{
    public override string DisplayType => "Binary";
    public override string DisplayValue => $"";


    public BinaryGFFNode(IGFFNode parent, string label) : base(parent, label)
    {
        FieldValue = new byte[0];
    }
    public BinaryGFFNode(IGFFNode parent, string label, byte[] value) : this(parent, label)
    {
        FieldValue = value;
    }
}

