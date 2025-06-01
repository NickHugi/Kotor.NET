using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class FieldLocalizedStringGFFNodeViewModel : IFieldGFFTreeNodeViewModel<LocalizedStringViewModel>
{
    public override string Type => "Localized String";
    public override string Value => FieldValue.StringRef == -1
        ? $"[{FieldValue.StringRef}]"
        : FieldValue.SubStrings.FirstOrDefault(x => x.Language == Language.English)?.Text?.ToString() ?? "";

    public FieldLocalizedStringGFFNodeViewModel(IGFFNodeViewModel parent, string label) : base(parent, label)
    {
        FieldValue = new();
        FieldValue.WhenAnyPropertyChanged().Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
        FieldValue.SubStrings.WhenAnyPropertyChanged().Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
    }
    public FieldLocalizedStringGFFNodeViewModel(IGFFNodeViewModel parent, string label, LocalizedStringViewModel value) : this(parent, label)
    {
        FieldValue = value;
    }
    public FieldLocalizedStringGFFNodeViewModel(IGFFNodeViewModel parent, string label, LocalisedString value) : this(parent, label)
    {
        FieldValue = new()
        {
            StringRef = value.StringRef,
            SubStrings = new(value.AllSubstrings().Select(x => new LocalizedSubStringViewModel()
            {
                Language = x.Language,
                Gender = x.Gender,
                Text = x.Text
            }))
        };
    }
}

