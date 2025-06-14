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

public class SetVector3Action : BaseSetNodeAction<Vector3GFFNodeViewModel, Vector3ViewModel?>
{
    public SetVector3Action(NodePath path, Vector3ViewModel? oldValue, Vector3ViewModel? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override Vector3GFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, Vector3ViewModel? value)
        => new Vector3GFFNodeViewModel(parentNode, Path.Tail, value);

    protected override void SetNewValue(Vector3GFFNodeViewModel node)
        => node.FieldValue = NewValue!;

    protected override void SetOldValue(Vector3GFFNodeViewModel node)
        => node.FieldValue = OldValue!;
}
