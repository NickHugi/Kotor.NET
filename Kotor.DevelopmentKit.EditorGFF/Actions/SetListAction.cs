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

public class SetListAction : BaseSetNodeAction<BaseGFFNodeViewModel, bool?>
{
    public SetListAction(NodePath path)
        : base(path, null, true)
    {
    }

    protected override ListGFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, bool? _)
    {
        return new ListGFFNodeViewModel(parentNode, Path.Tail);
    }

    protected override void SetNewValue(BaseGFFNodeViewModel node)
    {
    }

    protected override void SetOldValue(BaseGFFNodeViewModel node)
    {
    }
}
