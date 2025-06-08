using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.EditorGFF.Models;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public abstract class BaseGFFNodeViewModel : ReactiveObject, IGFFNodeViewModel
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

    public IGFFNodeViewModel? Parent { get; }
    public abstract ReadOnlyObservableCollection<BaseGFFNodeViewModel> Children { get; }

    public IEnumerable<string> Path
    {
        get
        {
            var path = new List<string>();
            IGFFNodeViewModel cursor = this;
            while (cursor.Parent is not null)
            {
                if ((BaseGFFNodeViewModel)cursor is ListStructGFFNodeViewModel structInList)
                {
                    path.Add(structInList.Children.IndexOf(structInList).ToString());
                }
                else
                {
                    path.Add(cursor.Label);
                }
                cursor = cursor.Parent;
            }
            path.Reverse();
            return path;
        }
    }

    public bool IsDeleted
    {
        get => !Parent?.Children?.Contains(this) ?? false;
    }


    public BaseGFFNodeViewModel(IGFFNodeViewModel? parent)
    {
        Parent = parent;
    }

    public abstract void Delete();

    public NodePath GetPathOf(IGFFNodeViewModel node)
    {
        var next = node;
        var path = new List<object>();

        while (next is not null)
        {
            if (next is BaseFieldGFFNodeViewModel fieldNode)
            {
                path.Add(next.Label);
            }
            else if (next is ListStructGFFNodeViewModel structInList)
            {
                var listNode = (FieldListGFFNodeViewModel)structInList.Parent;
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
            cursor = cursor.NavigateTo<BaseGFFNodeViewModel>([remaining.First()]);
            remaining = new NodePath(path.Skip(count));
        }

        var existing = new NodePath(path.Take(count));
        var missing = new NodePath(path.Skip(count));

        return (existing, missing);
    }

    public TTargetNode? NavigateTo<TTargetNode>(params NodePath[] path) where TTargetNode : BaseGFFNodeViewModel
    {
        var fullPath = path.SelectMany(x => x);
        var node = NavigateTo<TTargetNode>(fullPath);
        return node;
    }
    public TTargetNode? NavigateTo<TTargetNode>(IEnumerable<object> path) where TTargetNode : BaseGFFNodeViewModel
    {
        IGFFNodeViewModel? node = this;

        foreach (var step in path)
        {
            if (step is int listIndex)
            {
                if (node is FieldListGFFNodeViewModel listNode)
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
                if (node is IStructGFFTreeNodeViewModel structNode)
                {
                    node = structNode.Children.OfType<BaseFieldGFFNodeViewModel>().FirstOrDefault(x => x.Label == fieldLabel);
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
