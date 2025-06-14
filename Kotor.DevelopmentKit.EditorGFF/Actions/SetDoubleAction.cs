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

public class SetDoubleAction : BaseSetNodeAction<DoubleGFFNodeViewModel, Double?>
{
    public SetDoubleAction(NodePath path, Double? oldValue, Double? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override DoubleGFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, Double? value)
        => new DoubleGFFNodeViewModel(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(DoubleGFFNodeViewModel node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(DoubleGFFNodeViewModel node)
        => node.FieldValue = OldValue!.Value;
}
