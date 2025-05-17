using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetStructAction : IAction<GFFResourceEditorViewModel>
{
    public BaseStructGFFTreeNodeViewModel Node { get; }
    public Int32 OldValue { get; }
    public Int32 NewValue { get; }

    public SetStructAction(BaseStructGFFTreeNodeViewModel node, Int32 oldValue, Int32 newValue)
    {
        Node = node;
        OldValue = oldValue;
        NewValue = newValue;
    }

    public void Apply(GFFResourceEditorViewModel data)
    {
        Node.StructID = NewValue;
    }

    public void Undo(GFFResourceEditorViewModel data)
    {
        Node.StructID = OldValue;
    }
}
