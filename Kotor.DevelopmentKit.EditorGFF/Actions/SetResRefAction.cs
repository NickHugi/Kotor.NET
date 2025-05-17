using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetVector3Action : IAction<GFFResourceEditorViewModel>
{
    public Vector3GFFTreeNodeViewModel Node { get; }
    public Vector3ViewModel OldValue { get; }
    public Vector3ViewModel NewValue { get; }

    public SetVector3Action(Vector3GFFTreeNodeViewModel node, Vector3ViewModel oldValue, Vector3ViewModel newValue)
    {
        Node = node;
        OldValue = oldValue.Clone();
        NewValue = newValue.Clone();
    }

    public void Apply(GFFResourceEditorViewModel data)
    {
        Node.FieldValue = NewValue;
    }

    public void Undo(GFFResourceEditorViewModel data)
    {
        Node.FieldValue = OldValue;
    }
}
