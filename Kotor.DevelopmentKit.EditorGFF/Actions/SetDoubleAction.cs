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

public class SetDoubleAction : BaseSetNodeAction<DoubleGFFNode, Double?>
{
    public SetDoubleAction(NodePath path, Double? oldValue, Double? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override DoubleGFFNode InstantiateNode(IGFFNode parentNode, Double? value)
        => new DoubleGFFNode(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(DoubleGFFNode node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(DoubleGFFNode node)
        => node.FieldValue = OldValue!.Value;
}
