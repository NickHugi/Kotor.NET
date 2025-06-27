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

public class SetUInt64Action : BaseSetNodeAction<UInt64GFFNode, UInt64?>
{
    public SetUInt64Action(NodePath path, UInt64? oldValue, UInt64? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override UInt64GFFNode InstantiateNode(IGFFNode parentNode, UInt64? value)
        => new UInt64GFFNode(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(UInt64GFFNode node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(UInt64GFFNode node)
        => node.FieldValue = OldValue!.Value;
}
