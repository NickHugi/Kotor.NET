using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
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
        data.NavigateTo<BaseFieldGFFNodeViewModel>(OldPath).Label = NewLabel;
    }

    public void Undo(GFFResourceEditorViewModel data)
    {
        data.NavigateTo<BaseFieldGFFNodeViewModel>(NewPath).Label = NewLabel;
    }
}
