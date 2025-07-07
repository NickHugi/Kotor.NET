using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetResRefAction : BaseSetNodeAction<ResRefGFFNode, ReactiveResRef?>
{
    public SetResRefAction(NodePath path, ReactiveResRef? oldValue, ReactiveResRef? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override ResRefGFFNode InstantiateNode(IGFFNode parentNode, ReactiveResRef? value)
        => new ResRefGFFNode(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(ResRefGFFNode node)
        => node.FieldValue = NewValue!;

    protected override void SetOldValue(ResRefGFFNode node)
        => node.FieldValue = OldValue!;
}
