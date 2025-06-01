using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetStructAction : BaseSetNodeAction<BaseGFFNodeViewModel, Int32?>
{
    public SetStructAction(IEnumerable<object> path, Int32? oldValue, Int32? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override BaseGFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, Int32? value)
    {
        if (parentNode is BaseGFFNodeViewModel rootNode)
        {
            return new FieldStructGFFNodeViewModel(parentNode, _name) { StructID = value.Value };
        }
        else if (parentNode is FieldListGFFNodeViewModel listNode)
        {
            return new ListStructGFFNodeViewModel(parentNode) { StructID = value.Value };
        }
        else if (parentNode is FieldStructGFFNodeViewModel structNode)
        {
            return new FieldStructGFFNodeViewModel(parentNode, _name) { StructID = value.Value };
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    protected override void SetNewValue(BaseGFFNodeViewModel node)
        => (node as IStructGFFTreeNodeViewModel).StructID = NewValue.Value;

    protected override void SetOldValue(BaseGFFNodeViewModel node)
        => (node as IStructGFFTreeNodeViewModel).StructID = OldValue.Value;
}
