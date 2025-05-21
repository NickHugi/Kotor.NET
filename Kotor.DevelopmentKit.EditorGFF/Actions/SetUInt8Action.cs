using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetUInt8Action : BaseSetNodeAction<UInt8GFFTreeNodeViewModel, byte?>
{
    public SetUInt8Action(IEnumerable<object> path, byte? oldValue, byte? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override UInt8GFFTreeNodeViewModel InstantiateNode(IGFFTreeNodeViewModel parentNode, byte? value)
        => new UInt8GFFTreeNodeViewModel((BaseStructGFFTreeNodeViewModel)parentNode, _name, (byte)value);

    protected override void SetNewValue(UInt8GFFTreeNodeViewModel node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(UInt8GFFTreeNodeViewModel node)
        => node.FieldValue = OldValue!.Value;
}
