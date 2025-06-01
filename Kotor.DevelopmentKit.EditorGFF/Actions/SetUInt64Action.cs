using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetUInt64Action : IAction<GFFResourceEditorViewModel>
{
    public FieldUInt64GFFNodeViewModel Node { get; }
    public UInt64 OldValue { get; }
    public UInt64 NewValue { get; }

    public SetUInt64Action(FieldUInt64GFFNodeViewModel node, UInt64 oldValue, UInt64 newValue)
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
