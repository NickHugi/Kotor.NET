using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetLocalizedStringAction : BaseSetNodeAction<FieldLocalizedStringGFFNodeViewModel, LocalizedStringViewModel?>
{
    public SetLocalizedStringAction(NodePath path, LocalizedStringViewModel? oldValue, LocalizedStringViewModel? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override FieldLocalizedStringGFFNodeViewModel InstantiateNode(IGFFNodeViewModel parentNode, LocalizedStringViewModel? value)
        => new FieldLocalizedStringGFFNodeViewModel(parentNode, Path.Tail, value);

    protected override void SetNewValue(FieldLocalizedStringGFFNodeViewModel node)
        => node.FieldValue = NewValue!;

    protected override void SetOldValue(FieldLocalizedStringGFFNodeViewModel node)
        => node.FieldValue = OldValue!;
}
