using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.Extensions;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetFieldLabel : IAction<GFFResourceEditorViewModel>
{
    public NodePath OldPath { get; }
    public NodePath NewPath => new NodePath(OldPath.SkipLast(1).Concat([NewLabel]));
    public string OldLabel { get; }
    public string NewLabel { get; }

    public SetFieldLabel(IEnumerable<object> path, string oldLabel, string newLabel)
    {
        OldPath = new NodePath(path);
        OldLabel = oldLabel;
        NewLabel = newLabel;
    }

    public void Apply(GFFResourceEditorViewModel data)
    {
        var node = data.RootNode.NavigateTo<BaseFieldGFFNode>(OldPath);
        node.Label = NewLabel.GetUniqueLabel(node.Parent as IStructGFFNode);
    }

    public void Undo(GFFResourceEditorViewModel data)
    {
        var node = data.RootNode.NavigateTo<BaseFieldGFFNode>(OldPath);
        node.Label = OldLabel;
    }
}
