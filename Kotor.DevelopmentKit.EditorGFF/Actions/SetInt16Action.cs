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

public class SetInt16Action : BaseSetNodeAction<Int16GFFNodeViewModel, Int16?>
{
    public SetInt16Action(NodePath path, Int16? oldValue, Int16? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override Int16GFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, Int16? value)
        => new Int16GFFNodeViewModel(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(Int16GFFNodeViewModel node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(Int16GFFNodeViewModel node)
        => node.FieldValue = OldValue!.Value;
}
