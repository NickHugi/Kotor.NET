using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetVector4Action : BaseSetNodeAction<FieldVector4GFFNodeViewModel, Vector4ViewModel?>
{
    public SetVector4Action(NodePath path, Vector4ViewModel? oldValue, Vector4ViewModel? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override FieldVector4GFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, Vector4ViewModel? value)
        => new FieldVector4GFFNodeViewModel(parentNode, Path.Tail, value);

    protected override void SetNewValue(FieldVector4GFFNodeViewModel node)
        => node.FieldValue = NewValue!;

    protected override void SetOldValue(FieldVector4GFFNodeViewModel node)
        => node.FieldValue = OldValue!;
}

