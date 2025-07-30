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

public class SetInt64Action : BaseSetNodeAction<Int64GFFNode, Int64?>
{
    public SetInt64Action(NodePath path, Int64? oldValue, Int64? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override Int64GFFNode InstantiateNode(IGFFNode parentNode, Int64? value)
        => new Int64GFFNode(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(Int64GFFNode node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(Int64GFFNode node)
        => node.FieldValue = OldValue!.Value;
}
