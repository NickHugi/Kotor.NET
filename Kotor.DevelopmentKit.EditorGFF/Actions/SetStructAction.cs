using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetStructAction : BaseSetNodeAction<BaseStructGFFTreeNodeViewModel, Int32?>
{
    public SetStructAction(IEnumerable<object> path, Int32? oldValue, Int32? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override BaseStructGFFTreeNodeViewModel InstantiateNode(IGFFTreeNodeViewModel parentNode, Int32? value)
    {
        if (parentNode is RootStructGFFTreeNodeViewModel rootNode)
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
            throw new NotSupportedException();
        }
    }

    protected override void SetNewValue(BaseStructGFFTreeNodeViewModel node)
        => node.StructID = NewValue.Value;

    protected override void SetOldValue(BaseStructGFFTreeNodeViewModel node)
        => node.StructID = OldValue.Value;
}

//{
//    public IEnumerable<object> Path { get; }
//    public Int32 OldValue { get; }
//    public Int32 NewValue { get; }

//    public SetStructAction(IEnumerable<object> path, Int32 oldValue, Int32 newValue)
//    {
//        Path = path;
//        OldValue = oldValue;
//        NewValue = newValue;
//    }

//    public void Apply(GFFResourceEditorViewModel data)
//    {
//        Node.StructID = NewValue;
//    }

//    public void Undo(GFFResourceEditorViewModel data)
//    {
//        Node.StructID = OldValue;
//    }
//}
