using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class DeleteFieldAction : IAction<GFFResourceEditorViewModel>
{
    public IFieldGFFNodeViewModel Node { get; }

    public DeleteFieldAction(IFieldGFFNodeViewModel node)
    {
        Node = node;
    }

    public void Apply(GFFResourceEditorViewModel data)
    {
        Node.Delete();
    }

    public void Undo(GFFResourceEditorViewModel data)
    {
        if (Node.Parent is IStructGFFTreeNodeViewModel vmStruct)
        {
            vmStruct.AddField(Node);
        }
    }
}

