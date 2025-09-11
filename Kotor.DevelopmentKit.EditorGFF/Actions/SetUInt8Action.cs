using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetUInt8Action : BaseSetNodeAction<UInt8GFFNode, byte?>
{
    public SetUInt8Action(NodePath path, byte? oldValue, byte? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override UInt8GFFNode InstantiateNode(IGFFNode parentNode, byte? value)
        => new UInt8GFFNode(parentNode, Path.Tail, (byte)value);

    protected override void SetNewValue(UInt8GFFNode node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(UInt8GFFNode node)
        => node.FieldValue = OldValue!.Value;
}
