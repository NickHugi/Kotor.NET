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

public class SetStructAction : BaseSetNodeAction<BaseGFFNode, Int32?>
{
    public SetStructAction(NodePath path, Int32? oldValue, Int32? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override BaseGFFNode InstantiateNode(IGFFNode parentNode, Int32? value)
    {
        if (parentNode is ListGFFNode listNode)
        {
            return new ListStructGFFNode(parentNode) { StructID = value.Value };
        }
        else if (parentNode is FieldStructGFFNode structNode)
        {
            return new FieldStructGFFNode(parentNode, Path.Tail) { StructID = value.Value };
        }
        if (parentNode is RootStructGFFNode rootNode)
        {
            return new FieldStructGFFNode(parentNode, Path.Tail) { StructID = value.Value };
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    protected override void SetNewValue(BaseGFFNode node)
        => (node as IStructGFFNode).StructID = NewValue.Value;

    protected override void SetOldValue(BaseGFFNode node)
        => (node as IStructGFFNode).StructID = OldValue.Value;
}
