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

public class SetUInt64Action : BaseSetNodeAction<UInt64GFFNodeViewModel, UInt64?>
{
    public SetUInt64Action(NodePath path, UInt64? oldValue, UInt64? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override UInt64GFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, UInt64? value)
        => new UInt64GFFNodeViewModel(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(UInt64GFFNodeViewModel node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(UInt64GFFNodeViewModel node)
        => node.FieldValue = OldValue!.Value;
}
