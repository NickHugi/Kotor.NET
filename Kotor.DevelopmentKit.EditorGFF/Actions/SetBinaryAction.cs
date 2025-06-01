using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetBinaryAction : IAction<GFFResourceEditorViewModel>
{
    public FieldBinaryGFFNodeViewModel Node { get; }
    public byte[] OldValue { get; }
    public byte[] NewValue { get; }

    public SetBinaryAction(FieldBinaryGFFNodeViewModel node, byte[] oldValue, byte[] newValue)
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

        if (OldValue is null)
        {
            Node.Delete();
        }
    }
}
