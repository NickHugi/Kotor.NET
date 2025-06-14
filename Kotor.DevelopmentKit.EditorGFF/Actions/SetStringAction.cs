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

public class SetStringAction : BaseSetNodeAction<StringGFFNodeViewModel, String?>
{
    public SetStringAction(NodePath path, String? oldValue, String? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override StringGFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, String? value)
        => new StringGFFNodeViewModel(parentNode, Path.Tail, value);

    protected override void SetNewValue(StringGFFNodeViewModel node)
        => node.FieldValue = NewValue!;

    protected override void SetOldValue(StringGFFNodeViewModel node)
        => node.FieldValue = OldValue!;
}
