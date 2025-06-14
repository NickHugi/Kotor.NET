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

public class SetInt64Action : BaseSetNodeAction<Int64GFFNodeViewModel, Int64?>
{
    public SetInt64Action(NodePath path, Int64? oldValue, Int64? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override Int64GFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, Int64? value)
        => new Int64GFFNodeViewModel(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(Int64GFFNodeViewModel node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(Int64GFFNodeViewModel node)
        => node.FieldValue = OldValue!.Value;
}
