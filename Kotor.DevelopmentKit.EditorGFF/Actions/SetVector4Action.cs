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

public class SetVector4Action : BaseSetNodeAction<Vector4GFFNode, Vector4ViewModel?>
{
    public SetVector4Action(NodePath path, Vector4ViewModel? oldValue, Vector4ViewModel? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override Vector4GFFNode InstantiateNode(IGFFNode parentNode, Vector4ViewModel? value)
        => new Vector4GFFNode(parentNode, Path.Tail, value);

    protected override void SetNewValue(Vector4GFFNode node)
        => node.FieldValue = NewValue!;

    protected override void SetOldValue(Vector4GFFNode node)
        => node.FieldValue = OldValue!;
}

