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

public class SetVector3Action : BaseSetNodeAction<Vector3GFFNode, Vector3ViewModel?>
{
    public SetVector3Action(NodePath path, Vector3ViewModel? oldValue, Vector3ViewModel? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override Vector3GFFNode InstantiateNode(IGFFNode parentNode, Vector3ViewModel? value)
        => new Vector3GFFNode(parentNode, Path.Tail, value);

    protected override void SetNewValue(Vector3GFFNode node)
        => node.FieldValue = NewValue!;

    protected override void SetOldValue(Vector3GFFNode node)
        => node.FieldValue = OldValue!;
}
