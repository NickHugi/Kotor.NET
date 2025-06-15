using System;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public abstract class BaseFieldGFFNodeViewModel : BaseGFFNodeViewModel
{
    public override string DisplayValue => "";
    public override bool CanEditLabel => true;

    private string _label = "";
    public override string Label
    {
        get => _label;
        set => this.RaiseAndSetIfChanged(ref _label, value);
    }

    private ReadOnlyObservableCollection<BaseGFFNodeViewModel> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFNodeViewModel> Children => _children;


    protected BaseFieldGFFNodeViewModel(IGFFNodeViewModel? parent) : base(parent)
    {
    }

    public override void Delete()
    {
        if (Parent is IStructGFFNodeViewModel parentStruct)
        {
            parentStruct.DeleteField(this);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}

public abstract class BaseFieldGFFNodeViewModel<T> : BaseFieldGFFNodeViewModel where T : notnull
{
    private T _fieldValue = default!;
    public T FieldValue
    {
        get => _fieldValue;
        set => this.RaiseAndSetIfChanged(ref _fieldValue, value);
    }

    public override string DisplayValue => FieldValue?.ToString() ?? "";


    public BaseFieldGFFNodeViewModel(IGFFNodeViewModel parent, string label) : base(parent)
    {
        Label = label;

        this.ObservableForProperty(x => x.Label).Subscribe(x => this.RaisePropertyChanged(nameof(Label)));
        this.ObservableForProperty(x => x.FieldValue).Subscribe(x => this.RaisePropertyChanged(nameof(DisplayValue)));
    }
}
