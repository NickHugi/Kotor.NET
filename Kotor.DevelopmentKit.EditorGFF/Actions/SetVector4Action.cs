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

public class SetVector4Action : IAction<GFFResourceEditorViewModel>
{
    public FieldVector4GFFNodeViewModel Node { get; }
    public Vector4ViewModel OldValue { get; }
    public Vector4ViewModel NewValue { get; }

    public SetVector4Action(FieldVector4GFFNodeViewModel node, Vector4ViewModel oldValue, Vector4ViewModel newValue)
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
