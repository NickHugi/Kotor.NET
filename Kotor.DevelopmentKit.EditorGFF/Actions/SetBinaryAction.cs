using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetBinaryAction : BaseSetNodeAction<BinaryGFFNode, byte[]>
{
    public SetBinaryAction(NodePath path, byte[]? oldValue, byte[] newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override BinaryGFFNode InstantiateNode(IGFFNode parentNode, byte[] value)
        => new BinaryGFFNode(parentNode, Path.Tail, value);

    protected override void SetNewValue(BinaryGFFNode node)
        => node.FieldValue = NewValue!;

    protected override void SetOldValue(BinaryGFFNode node)
        => node.FieldValue = OldValue!;
}
