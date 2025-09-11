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

public class SetStringAction : BaseSetNodeAction<StringGFFNode, String?>
{
    public SetStringAction(NodePath path, String? oldValue, String? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override StringGFFNode InstantiateNode(IGFFNode parentNode, String? value)
        => new StringGFFNode(parentNode, Path.Tail, value);

    protected override void SetNewValue(StringGFFNode node)
        => node.FieldValue = NewValue!;

    protected override void SetOldValue(StringGFFNode node)
        => node.FieldValue = OldValue!;
}
