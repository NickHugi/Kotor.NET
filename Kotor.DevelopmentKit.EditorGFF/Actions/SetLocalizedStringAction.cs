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

public class SetLocalizedStringAction : IAction<GFFResourceEditorViewModel>
{
    public FieldLocalizedStringGFFNodeViewModel Node { get; }
    public LocalizedStringViewModel OldValue { get; }
    public LocalizedStringViewModel NewValue { get; }

    public SetLocalizedStringAction(FieldLocalizedStringGFFNodeViewModel node, LocalizedStringViewModel oldValue, LocalizedStringViewModel newValue)
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
