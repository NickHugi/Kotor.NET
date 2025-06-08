using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.Extensions;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetFieldLabel : IAction<GFFResourceEditorViewModel>
{
    public IEnumerable<object> OldPath { get; }
    public IEnumerable<object> NewPath => OldPath.SkipLast(1).Concat([NewLabel]);
    public string OldLabel { get; }
    public string NewLabel { get; }

    public SetFieldLabel(IEnumerable<object> path, string oldLabel, string newLabel)
    {
        OldPath = path;
        OldLabel = oldLabel;
        NewLabel = newLabel;
    }

    public void Apply(GFFResourceEditorViewModel data)
    {
        var node = data.RootNode.NavigateTo<BaseFieldGFFNodeViewModel>(OldPath);
        node.Label = NewLabel.GetUniqueLabel(node.Parent as IStructGFFTreeNodeViewModel);
    }

    public void Undo(GFFResourceEditorViewModel data)
    {
        var node = data.RootNode.NavigateTo<BaseFieldGFFNodeViewModel>(OldPath);
        node.Label = OldLabel;
    }
}
