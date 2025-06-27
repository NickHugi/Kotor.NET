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

public class SetUInt16Action : BaseSetNodeAction<UInt16GFFNode, UInt16?>
{
    public SetUInt16Action(NodePath path, UInt16? oldValue, UInt16? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override UInt16GFFNode InstantiateNode(IGFFNode parentNode, UInt16? value)
        => new UInt16GFFNode(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(UInt16GFFNode node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(UInt16GFFNode node)
        => node.FieldValue = OldValue!.Value;
}
