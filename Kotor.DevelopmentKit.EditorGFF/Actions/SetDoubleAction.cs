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

public class SetDoubleAction : BaseSetNodeAction<FieldDoubleGFFNodeViewModel, Double?>
{
    public SetDoubleAction(NodePath path, Double? oldValue, Double? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override FieldDoubleGFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, Double? value)
        => new FieldDoubleGFFNodeViewModel(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(FieldDoubleGFFNodeViewModel node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(FieldDoubleGFFNodeViewModel node)
        => node.FieldValue = OldValue!.Value;
}
