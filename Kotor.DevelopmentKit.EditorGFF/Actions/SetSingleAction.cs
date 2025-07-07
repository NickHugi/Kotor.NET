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

public class SetSingleAction : BaseSetNodeAction<SingleGFFNode, Single?>
{
    public SetSingleAction(NodePath path, Single? oldValue, Single? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override SingleGFFNode InstantiateNode(IGFFNode parentNode, Single? value)
        => new SingleGFFNode(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(SingleGFFNode node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(SingleGFFNode node)
        => node.FieldValue = OldValue!.Value;
}

