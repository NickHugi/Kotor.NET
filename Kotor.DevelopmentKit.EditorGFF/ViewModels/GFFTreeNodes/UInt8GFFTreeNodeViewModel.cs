using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class UInt8GFFTreeNodeViewModel : ReactiveObject, IFieldGFFTreeNodeViewModel<byte>
{
    private string _label = "";
    public string Label
    {
        get => _label;
        set => this.RaiseAndSetIfChanged(ref _label, value);
    }

    public bool CanEditLabel => true;

    private byte _fieldValue;
    public byte FieldValue
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

    public string Type => "UInt8";
    public string Value => FieldValue.ToString();

    public UInt8GFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, byte value = 0)
    {
        Parent = parent;
        Label = label;
        FieldValue = value;

        this.ObservableForProperty(x => x.FieldValue).Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
    }

    public void Delete()
    {
        ((BaseStructGFFTreeNodeViewModel)Parent).DeleteField(this);
    }
}

