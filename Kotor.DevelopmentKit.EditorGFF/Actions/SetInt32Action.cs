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

public class SetInt32Action : BaseSetNodeAction<Int32GFFNode, Int32?>
{
    public SetInt32Action(NodePath path, Int32? oldValue, Int32? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override Int32GFFNode InstantiateNode(IGFFNode parentNode, Int32? value)
        => new Int32GFFNode(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(Int32GFFNode node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(Int32GFFNode node)
        => node.FieldValue = OldValue!.Value;
}
