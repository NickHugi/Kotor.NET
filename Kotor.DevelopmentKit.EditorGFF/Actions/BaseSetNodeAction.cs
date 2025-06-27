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

public abstract class BaseSetNodeAction<TNodeViewModel, TValueViewModel>
    : IAction<GFFResourceEditorViewModel>
    where TNodeViewModel : BaseGFFNode
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
            var newParentPath = new NodePath(Path.SkipLast(1));
            var parent = data.RootNode.NavigateTo<BaseGFFNode>(newParentPath);
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
            var newParentPath = new NodePath(Path.SkipLast(1));
            var parent = data.RootNode.NavigateTo<BaseGFFNode>(newParentPath) ?? throw new InvalidOperationException();
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

    protected TNodeViewModel CreateNode(BaseGFFNode parentNode, TValueViewModel value)
    {
        if (parentNode is IStructGFFNode structNode)
        {
            var newNode = InstantiateNode(structNode, value);
            structNode.AddField(newNode as BaseFieldGFFNode);
            return newNode;
        }
        else if (parentNode is ListGFFNode listNode)
        {
            var newNode = InstantiateNode(parentNode, value);
            listNode.AddStruct(newNode as ListStructGFFNode);
            return newNode;
        }
        else
        {
            throw new ArgumentException();
        }
    }

    protected abstract TNodeViewModel InstantiateNode(IGFFNode parentNode, TValueViewModel value);
    protected abstract void SetNewValue(TNodeViewModel data);
    protected abstract void SetOldValue(TNodeViewModel data);
}
