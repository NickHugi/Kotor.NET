using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class FieldInt32GFFNodeViewModel : BaseFieldGFFNodeViewModel<int>
{
    public override string DisplayType => "Int32";

    public FieldInt32GFFNodeViewModel(IGFFNodeViewModel parent, string label, int value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

