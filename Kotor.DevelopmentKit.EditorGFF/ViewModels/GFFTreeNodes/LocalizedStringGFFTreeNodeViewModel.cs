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

public class LocalizedStringGFFTreeNodeViewModel : ReactiveObject, IFieldGFFTreeNodeViewModel<LocalizedStringViewModel>
{
    private string _label = "";
    public string Label
    {
        get => _label;
        set => this.RaiseAndSetIfChanged(ref _label, value);
    }

    public bool CanEditLabel => true;

    private LocalizedStringViewModel _fieldValue;
    public LocalizedStringViewModel FieldValue
    {
        get => _fieldValue;
        set => this.RaiseAndSetIfChanged(ref _fieldValue, value);
    }

    private bool _expanded;
    public bool Expanded
    {
        get => _expanded;
        set => this.RaiseAndSetIfChanged(ref _expanded, value);
    }

    public IGFFTreeNodeViewModel Parent { get; }

    private ReadOnlyObservableCollection<IGFFTreeNodeViewModel> _children = new([]);
    public ReadOnlyObservableCollection<IGFFTreeNodeViewModel> Children => _children;

    public string Type => "Localized String";
    public string Value => FieldValue.StringRef == -1
        ? $"[{FieldValue.StringRef}]"
        : FieldValue.SubStrings.FirstOrDefault(x => x.Language == Language.English)?.Text?.ToString() ?? "";

    public LocalizedStringGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label)
    {
        Parent = parent;
        Label = label;
        _fieldValue = new();

        this.ObservableForProperty(x => x.Label).Subscribe(x => this.RaisePropertyChanged(nameof(Label)));
        this.ObservableForProperty(x => x.FieldValue).Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
        FieldValue.WhenAnyPropertyChanged().Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
        FieldValue.SubStrings.WhenAnyPropertyChanged().Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
    }
    public LocalizedStringGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, LocalizedStringViewModel value) : this(parent, label)
    {
        FieldValue = value;
    }

    public void Delete()
    {
        ((IStructGFFTreeNodeViewModel)Parent).DeleteField(this);
    }
}

