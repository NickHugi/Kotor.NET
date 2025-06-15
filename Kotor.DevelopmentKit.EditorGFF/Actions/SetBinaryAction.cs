using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetBinaryAction : BaseSetNodeAction<BinaryGFFNodeViewModel, byte[]>
{
    public SetBinaryAction(NodePath path, byte[]? oldValue, byte[] newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override BinaryGFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, byte[] value)
        => new BinaryGFFNodeViewModel(parentNode, Path.Tail, value);

    protected override void SetNewValue(BinaryGFFNodeViewModel node)
        => node.FieldValue = NewValue!;

    protected override void SetOldValue(BinaryGFFNodeViewModel node)
        => node.FieldValue = OldValue!;
}
