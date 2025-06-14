using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetUInt32Action : BaseSetNodeAction<UInt32GFFNodeViewModel, UInt32?>
{
    public SetUInt32Action(NodePath path, UInt32? oldValue, UInt32? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override UInt32GFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, UInt32? value)
        => new UInt32GFFNodeViewModel(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(UInt32GFFNodeViewModel node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(UInt32GFFNodeViewModel node)
        => node.FieldValue = OldValue!.Value;
}
