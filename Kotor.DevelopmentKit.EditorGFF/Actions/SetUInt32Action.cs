using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetUInt32Action : IAction<GFFResourceEditorViewModel>
{
    public FieldUInt32GFFNodeViewModel Node { get; }
    public UInt32 OldValue { get; }
    public UInt32 NewValue { get; }

    public SetUInt32Action(FieldUInt32GFFNodeViewModel node, UInt32 oldValue, UInt32 newValue)
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
