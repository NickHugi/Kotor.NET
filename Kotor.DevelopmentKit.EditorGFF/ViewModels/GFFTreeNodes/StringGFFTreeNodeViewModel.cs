using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class StringGFFTreeNodeViewModel : ReactiveObject, IFieldGFFTreeNodeViewModel<string>
{
    private string _label = "";
    public string Label
    {
        get => _label;
        set => this.RaiseAndSetIfChanged(ref _label, value);
    }

    public bool CanEditLabel => true;

    private string _fieldValue;
    public string FieldValue
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

    public string Type => "String";
    public string Value => FieldValue.Replace("\n", "").Replace("\r", "").ToString();

    public StringGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, string value = "")
    {
        Parent = parent;
        Label = label;
        _fieldValue = value;

        this.ObservableForProperty(x => x.Label).Subscribe(x => this.RaisePropertyChanged(nameof(Label)));
        this.ObservableForProperty(x => x.FieldValue).Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
    }

    public void Delete()
    {
        ((BaseStructGFFTreeNodeViewModel)Parent).DeleteField(this);
    }
}

