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

public class SetSingleAction : BaseSetNodeAction<SingleGFFNodeViewModel, Single?>
{
    public SetSingleAction(NodePath path, Single? oldValue, Single? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override SingleGFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, Single? value)
        => new SingleGFFNodeViewModel(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(SingleGFFNodeViewModel node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(SingleGFFNodeViewModel node)
        => node.FieldValue = OldValue!.Value;
}

