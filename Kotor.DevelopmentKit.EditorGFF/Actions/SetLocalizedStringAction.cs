using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;

namespace Kotor.DevelopmentKit.EditorGFF.Actions;

public class SetLocalizedStringAction : BaseSetNodeAction<LocalizedStringGFFNode, ReactiveLocalizedString?>
{
    public SetLocalizedStringAction(NodePath path, ReactiveLocalizedString? oldValue, ReactiveLocalizedString? newValue)
        : base(path, oldValue, newValue)
    {
    }

    protected override LocalizedStringGFFNode InstantiateNode(IGFFNode parentNode, ReactiveLocalizedString? value)
        => new LocalizedStringGFFNode(parentNode, Path.Tail, value);

    protected override void SetNewValue(LocalizedStringGFFNode node)
        => node.FieldValue = NewValue!;

    protected override void SetOldValue(LocalizedStringGFFNode node)
        => node.FieldValue = OldValue!;
}
