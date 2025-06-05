using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class FieldSingleGFFNodeViewModel : BaseFieldGFFNodeViewModel<float>
{
    public override string DisplayType => "Single";

    public FieldSingleGFFNodeViewModel(IGFFNodeViewModel parent, string label, float value = 0) : base(parent, label)
    {
        FieldValue = value;
    }
}

