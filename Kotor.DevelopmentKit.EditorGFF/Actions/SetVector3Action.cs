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

public class SetVector3Action : BaseSetNodeAction<FieldVector3GFFNodeViewModel, Vector3ViewModel?>
{
    public SetVector3Action(NodePath path, Vector3ViewModel? oldValue, Vector3ViewModel? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override FieldVector3GFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, Vector3ViewModel? value)
        => new FieldVector3GFFNodeViewModel(parentNode, Path.Tail, value);

    protected override void SetNewValue(FieldVector3GFFNodeViewModel node)
        => node.FieldValue = NewValue!;

    protected override void SetOldValue(FieldVector3GFFNodeViewModel node)
        => node.FieldValue = OldValue!;
}
