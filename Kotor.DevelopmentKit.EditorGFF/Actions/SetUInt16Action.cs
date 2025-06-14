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

public class SetUInt16Action : BaseSetNodeAction<UInt16GFFNodeViewModel, UInt16?>
{
    public SetUInt16Action(NodePath path, UInt16? oldValue, UInt16? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override UInt16GFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, UInt16? value)
        => new UInt16GFFNodeViewModel(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(UInt16GFFNodeViewModel node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(UInt16GFFNodeViewModel node)
        => node.FieldValue = OldValue!.Value;
}
