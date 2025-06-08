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

public class SetInt32Action : BaseSetNodeAction<FieldInt32GFFNodeViewModel, Int32?>
{
    public SetInt32Action(NodePath path, Int32? oldValue, Int32? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override FieldInt32GFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, Int32? value)
        => new FieldInt32GFFNodeViewModel(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(FieldInt32GFFNodeViewModel node)
        => node.FieldValue = NewValue!.Value;

    protected override void SetOldValue(FieldInt32GFFNodeViewModel node)
        => node.FieldValue = OldValue!.Value;
}
