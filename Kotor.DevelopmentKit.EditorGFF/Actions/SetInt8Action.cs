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

public class SetInt8Action : BaseSetNodeAction<Int8GFFNode, sbyte?>
{
    public SetInt8Action(NodePath path, sbyte? oldValue, sbyte? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override Int8GFFNode InstantiateNode(IGFFNode parentNode, sbyte? value)
        => new Int8GFFNode(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(Int8GFFNode node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(Int8GFFNode node)
        => node.FieldValue = OldValue!.Value;
}
