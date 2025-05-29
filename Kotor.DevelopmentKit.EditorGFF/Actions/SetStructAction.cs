using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetStructAction : BaseSetNodeAction<BaseGFFTreeNodeViewModel, Int32?>
{
    public SetStructAction(IEnumerable<object> path, Int32? oldValue, Int32? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override BaseGFFTreeNodeViewModel InstantiateNode(IGFFTreeNodeViewModel parentNode, Int32? value)
    {
        if (parentNode is BaseGFFTreeNodeViewModel rootNode)
        {
            return new StructGFFTreeNodeViewModel(parentNode, _name) { StructID = value.Value };
        }
        else if (parentNode is ListGFFTreeNodeViewModel listNode)
        {
            return new StructInListGFFTreeNodeViewModel(parentNode) { StructID = value.Value };
        }
        else if (parentNode is StructGFFTreeNodeViewModel structNode)
        {
            return new StructGFFTreeNodeViewModel(parentNode, _name) { StructID = value.Value };
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    protected override void SetNewValue(BaseGFFTreeNodeViewModel node)
        => (node as BaseStructGFFTreeNodeViewModel).StructID = NewValue.Value;

    protected override void SetOldValue(BaseGFFTreeNodeViewModel node)
        => (node as BaseStructGFFTreeNodeViewModel).StructID = OldValue.Value;
}
