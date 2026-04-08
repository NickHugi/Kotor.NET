using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class DeleteFieldAction : IAction<GFFResourceEditorViewModel>
{
    public BaseGFFNode Node { get; }

    public DeleteFieldAction(BaseGFFNode node)
    {
        Node = node;
    }

    public void Apply(GFFResourceEditorViewModel data)
    {
        Node.Delete();
    }

    public void Undo(GFFResourceEditorViewModel data)
    {
        if (Node.Parent is IStructGFFNode structNode && Node is BaseFieldGFFNode fieldNode)
        {
            structNode.AddField(fieldNode);
        }
        else if (Node.Parent is ListGFFNode listNode && Node is ListStructGFFNode childStructNode)
        {
            listNode.AddStruct(childStructNode);
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}

