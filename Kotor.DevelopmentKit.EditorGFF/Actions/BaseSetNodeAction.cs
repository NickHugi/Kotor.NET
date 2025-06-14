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

public abstract class BaseSetNodeAction<TNodeViewModel, TValueViewModel>
    : IAction<GFFResourceEditorViewModel>
    where TNodeViewModel : BaseGFFNodeViewModel
{
    public NodePath Path { get; }
    public TValueViewModel OldValue { get; }
    public TValueViewModel NewValue { get; }

    public BaseSetNodeAction(NodePath path, TValueViewModel oldValue, TValueViewModel newValue)
    {
        Path = path;
        OldValue = oldValue;
        NewValue = newValue;
    }

    public void Apply(GFFResourceEditorViewModel data)
    {
        var node = data.RootNode.NavigateTo<TNodeViewModel>(Path);

        if (node is null)
        {
            var parent = data.RootNode.NavigateTo<BaseGFFNodeViewModel>(Path.SkipLast(1));
            CreateNode(parent, NewValue);
        }
        else
        {
            if (NewValue is null)
            {
                node.Delete();
            }
            else
            {
                SetNewValue(node);
            }
        }
    }

    public void Undo(GFFResourceEditorViewModel data)
    {
        var node = data.RootNode.NavigateTo<TNodeViewModel>(Path);

        if (node is null)
        {
            var parent = data.RootNode.NavigateTo<BaseGFFNodeViewModel>(Path.SkipLast(1));
            CreateNode(parent, OldValue);
        }
        else
        {
            if (OldValue is null)
            {
                node.Delete();
            }
            else
            {
                SetOldValue(node);
            }
        }
    }

    protected TNodeViewModel CreateNode(BaseGFFNodeViewModel parentNode, TValueViewModel value)
    {
        if (parentNode is IStructGFFTreeNodeViewModel structNode)
        {
            var newNode = InstantiateNode(structNode, value);
            structNode.AddField(newNode as BaseFieldGFFNodeViewModel);
            return newNode;
        }
        else if (parentNode is ListGFFNodeViewModel listNode)
        {
            var newNode = InstantiateNode(parentNode, value);
            listNode.AddStruct(newNode as ListStructGFFNodeViewModel);
            return newNode;
        }
        else
        {
            throw new ArgumentException();
        }
    }

    protected abstract TNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, TValueViewModel value);
    protected abstract void SetNewValue(TNodeViewModel data);
    protected abstract void SetOldValue(TNodeViewModel data);
}
