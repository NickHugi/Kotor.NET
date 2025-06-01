using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetStringAction : IAction<GFFResourceEditorViewModel>
{
    public FieldStringGFFNodeViewModel Node { get; }
    public String OldValue { get; }
    public String NewValue { get; }

    public SetStringAction(FieldStringGFFNodeViewModel node, String oldValue, String newValue)
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
