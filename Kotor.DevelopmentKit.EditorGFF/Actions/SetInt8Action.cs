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

public class SetInt8Action : BaseSetNodeAction<Int8GFFNodeViewModel, sbyte?>
{
    public SetInt8Action(NodePath path, sbyte? oldValue, sbyte? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override Int8GFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, sbyte? value)
        => new Int8GFFNodeViewModel(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(Int8GFFNodeViewModel node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(Int8GFFNodeViewModel node)
        => node.FieldValue = OldValue!.Value;
}
