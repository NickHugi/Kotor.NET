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

public class SetUInt8Action : BaseSetNodeAction<FieldUInt8GFFNodeViewModel, byte?>
{
    public SetUInt8Action(NodePath path, byte? oldValue, byte? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override FieldUInt8GFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, byte? value)
        => new FieldUInt8GFFNodeViewModel(parentNode, Path.Tail, (byte)value);

    protected override void SetNewValue(FieldUInt8GFFNodeViewModel node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(FieldUInt8GFFNodeViewModel node)
        => node.FieldValue = OldValue!.Value;
}
