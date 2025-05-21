using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public abstract class BaseSetNodeAction<TNodeViewModel, TValueViewModel>
    : IAction<GFFResourceEditorViewModel>
    where TNodeViewModel : IGFFTreeNodeViewModel
{
    public IEnumerable<object> Path { get; }
    public TValueViewModel OldValue { get; }
    public TValueViewModel NewValue { get; }

    protected string _name => Path.Last().ToString();

    public BaseSetNodeAction(IEnumerable<object> path, TValueViewModel oldValue, TValueViewModel newValue)
    {
        Path = path;
        OldValue = oldValue;
        NewValue = newValue;
    }

    public void Apply(GFFResourceEditorViewModel data)
    {
        var node = data.NavigateTo<TNodeViewModel>(Path);

        if (NewValue is null)
        {
           node!.Delete();
        }
        else
        {
            if (node is null)
            {
                var parentNode = data.FillPath(Path.SkipLast(1));
                node = CreateNode(parentNode, NewValue);
            }
            else
            {
                SetNewValue(node);
            }
        }
    }

    public void Undo(GFFResourceEditorViewModel data)
    {
        var node = data.NavigateTo<TNodeViewModel>(Path);

        if (OldValue is null)
        {
            node!.Delete();
        }
        else
        {
            if (node is null)
            {
                var parentNode = data.FillPath(Path.SkipLast(1));
                node = CreateNode(parentNode, OldValue);
            }
            else
            {
                SetOldValue(node);
            }
        }
    }

    protected TNodeViewModel CreateNode(IGFFTreeNodeViewModel parentNode, TValueViewModel value)
    {
        if (parentNode is BaseStructGFFTreeNodeViewModel structNode)
        {
            var newNode = InstantiateNode(structNode, value);
            structNode.AddField((IFieldGFFTreeNodeViewModel)newNode);
            return newNode;
        }
        else if (parentNode is ListGFFTreeNodeViewModel listNode)
        {
            var newNode = InstantiateNode(parentNode, value);
            listNode.AddStruct((StructInListGFFTreeNodeViewModel)(IFieldGFFTreeNodeViewModel)newNode);
            return newNode;
        }
        else
        {
            throw new ArgumentException();
        }
    }

    protected abstract TNodeViewModel InstantiateNode(IGFFTreeNodeViewModel parentNode, TValueViewModel value);
    protected abstract void SetNewValue(TNodeViewModel data);
    protected abstract void SetOldValue(TNodeViewModel data);
}
