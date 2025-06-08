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

public class SetBinaryAction : BaseSetNodeAction<FieldBinaryGFFNodeViewModel, byte[]>
{
    public SetBinaryAction(NodePath path, byte[] oldValue, byte[] newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override FieldBinaryGFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, byte[] value)
        => new FieldBinaryGFFNodeViewModel(parentNode, Path.Tail, value);

    protected override void SetNewValue(FieldBinaryGFFNodeViewModel node)
        => node.FieldValue = NewValue!;

    protected override void SetOldValue(FieldBinaryGFFNodeViewModel node)
        => node.FieldValue = OldValue!;
}
