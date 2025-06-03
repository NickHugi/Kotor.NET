using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class FieldInt16GFFNodeViewModel : BaseFieldGFFTreeNodeViewModel<short>
{
    public override string DisplayType => "Int16";

    public FieldInt16GFFNodeViewModel(IGFFNodeViewModel parent, string label, short value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

