using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.EditorGFF.Models;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public abstract class BaseGFFNode : ReactiveObject, IGFFNode
{
    public abstract string Label { get; set; }
    public abstract bool CanEditLabel { get; }

    public abstract string DisplayType { get; }
    public abstract string DisplayValue { get; }

    private bool _expanded = true;
    public bool Expanded
    {
        get => _expanded;
        set => this.RaiseAndSetIfChanged(ref _expanded, value);
    }

    public int Version { get; set; }
    public int SavedVersion { get; set; }

    public IGFFNode? Parent { get; }
    public abstract ReadOnlyObservableCollection<BaseGFFNode> Children { get; }

    public NodePath Path
    {
        get
        {
            var path = new List<object>();
            IGFFNode cursor = this;
            while (cursor.Parent is not null)
            {
                if ((BaseGFFNode)cursor is ListStructGFFNode structInList)
                {
                    path.Add(cursor.Parent.Children.IndexOf(structInList));
                }
                else
                {
                    path.Add(cursor.Label);
                }
                cursor = cursor.Parent;
            }
            path.Reverse();
            return new(path);
        }
    }

    public bool IsDeleted
    {
        get => !Parent?.Children?.Contains(this) ?? false;
    }


    public BaseGFFNode(IGFFNode? parent)
    {
        Parent = parent;
    }

    public abstract void Delete();

    public NodePath GetPathOf(IGFFNode node)
    {
        var next = node;
        var path = new List<object>();

        while (next is not null)
        {
            if (next is BaseFieldGFFNode fieldNode)
            {
                path.Add(next.Label);
            }
            else if (next is ListStructGFFNode structInList)
            {
                var listNode = (ListGFFNode)structInList.Parent;
                path.Add(listNode.Children.IndexOf(structInList));
            }

            next = next.Parent;
        }

        path.Reverse();

        return new(path);
    }

    public (NodePath Existing, NodePath Missing) SplitPath(NodePath path)
    {
        var remaining = new NodePath(path);
        var cursor = this;
        var count = 0;

        while (remaining.Count() > 0 && cursor is not null)
        {
            count++;
            cursor = cursor.NavigateTo<BaseGFFNode>([remaining.First()]);
            remaining = new NodePath(path.Skip(count));
        }

        var existing = new NodePath(path.Take(count));
        var missing = new NodePath(path.Skip(count));

        return (existing, missing);
    }

    public TTargetNode? NavigateTo<TTargetNode>(params NodePath[] path) where TTargetNode : BaseGFFNode
    {
        var fullPath = path.SelectMany(x => x);
        var node = NavigateTo<TTargetNode>(fullPath);
        return node;
    }
    private TTargetNode? NavigateTo<TTargetNode>(IEnumerable<object> path) where TTargetNode : BaseGFFNode
    {
        IGFFNode? node = this;

        foreach (var step in path)
        {
            if (step is int listIndex)
            {
                if (node is ListGFFNode listNode)
                {
                    node = listNode.Children.ElementAtOrDefault(listIndex);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else if (step is string fieldLabel)
            {
                if (node is IStructGFFNode structNode)
                {
                    node = structNode.Children.OfType<BaseFieldGFFNode>().FirstOrDefault(x => x.Label == fieldLabel);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        if (node is null)
        {
            return default;
        }
        else if (node is TTargetNode targetNode)
        {
            return targetNode;
        }
        else
        {
            throw new ArgumentException();
        }
    }
}
