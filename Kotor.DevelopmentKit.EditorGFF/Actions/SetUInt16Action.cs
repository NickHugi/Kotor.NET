using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetUInt16Action : IAction<GFFResourceEditorViewModel>
{
    public IEnumerable<object> Path { get; }
    public UInt16 OldValue { get; }
    public UInt16 NewValue { get; }

    public SetUInt16Action(IEnumerable<object> path, UInt16 oldValue, UInt16 newValue)
    {
        Path = path;
        OldValue = oldValue;
        NewValue = newValue;
    }

    public void Apply(GFFResourceEditorViewModel data)
    {
        data.NavigateTo<UInt16GFFTreeNodeViewModel>(Path)!.FieldValue = NewValue;
    }

    public void Undo(GFFResourceEditorViewModel data)
    {
        data.NavigateTo<UInt16GFFTreeNodeViewModel>(Path)!.FieldValue = NewValue;
    }
}
