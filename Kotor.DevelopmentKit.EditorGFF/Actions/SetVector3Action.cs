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

public class SetResRefAction : IAction<GFFResourceEditorViewModel>
{
    public FieldResRefGFFNodeViewModel Node { get; }
    public ResRefViewModel OldValue { get; }
    public ResRefViewModel NewValue { get; }

    public SetResRefAction(FieldResRefGFFNodeViewModel node, ResRefViewModel oldValue, ResRefViewModel newValue)
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
