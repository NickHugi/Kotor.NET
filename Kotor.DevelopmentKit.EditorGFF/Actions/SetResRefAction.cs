using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetResRefAction : BaseSetNodeAction<ResRefGFFNodeViewModel, ResRefViewModel?>
{
    public SetResRefAction(NodePath path, ResRefViewModel? oldValue, ResRefViewModel? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override ResRefGFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, ResRefViewModel? value)
        => new ResRefGFFNodeViewModel(parentNode, Path.Tail, value.Value);

    protected override void SetNewValue(ResRefGFFNodeViewModel node)
        => node.FieldValue = NewValue!;

    protected override void SetOldValue(ResRefGFFNodeViewModel node)
        => node.FieldValue = OldValue!;
}
