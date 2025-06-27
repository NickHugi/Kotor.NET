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

public class SetUInt32Action : BaseSetNodeAction<UInt32GFFNode, UInt32?>
{
    public SetUInt32Action(NodePath path, UInt32? oldValue, UInt32? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override UInt32GFFNode InstantiateNode(IGFFNode parentNode, UInt32? value)
        => new UInt32GFFNode(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(UInt32GFFNode node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(UInt32GFFNode node)
        => node.FieldValue = OldValue!.Value;
}
