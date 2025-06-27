using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetListAction : BaseSetNodeAction<BaseGFFNode, bool?>
{
    public SetListAction(NodePath path)
        : base(path, null, true)
    {
    }

    protected override ListGFFNode InstantiateNode(IGFFNode parentNode, bool? _)
    {
        return new ListGFFNode(parentNode, Path.Tail);
    }

    protected override void SetNewValue(BaseGFFNode node)
    {
    }

    protected override void SetOldValue(BaseGFFNode node)
    {
    }
}
