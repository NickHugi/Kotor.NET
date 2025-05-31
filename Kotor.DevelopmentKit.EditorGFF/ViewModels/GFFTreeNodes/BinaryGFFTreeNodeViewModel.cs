using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class BinaryGFFTreeNodeViewModel : IFieldGFFTreeNodeViewModel<byte[]>
{
    public override string Type => "Binary";
    public override string Value => $"";


    public BinaryGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label) : base(parent, label)
    {
        FieldValue = new byte[0];
    }
    public BinaryGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, byte[] value) : this(parent, label)
    {
        FieldValue = value;
    }
}

