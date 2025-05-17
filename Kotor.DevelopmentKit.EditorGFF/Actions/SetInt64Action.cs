using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetInt64Action : IAction<GFFResourceEditorViewModel>
{
    public Int64GFFTreeNodeViewModel Node { get; }
    public Int64 OldValue { get; }
    public Int64 NewValue { get; }

    public SetInt64Action(Int64GFFTreeNodeViewModel node, Int64 oldValue, Int64 newValue)
    {
        Node = node;
        OldValue = oldValue;
        NewValue = newValue;
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
