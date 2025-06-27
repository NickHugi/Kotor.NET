using System;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public abstract class BaseFieldGFFNode : BaseGFFNode
{
    public override string DisplayValue => "";
    public override bool CanEditLabel => true;

    private string _label = "";
    public override string Label
    {
        get => _label;
        set => this.RaiseAndSetIfChanged(ref _label, value);
    }

    private ReadOnlyObservableCollection<BaseGFFNode> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFNode> Children => _children;


    protected BaseFieldGFFNode(IGFFNode? parent) : base(parent)
    {
    }

    public override void Delete()
    {
        if (Parent is IStructGFFNode parentStruct)
        {
            parentStruct.DeleteField(this);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}

public abstract class BaseFieldGFFNodeViewModel<T> : BaseFieldGFFNode where T : notnull
{
    private T _fieldValue = default!;
    public T FieldValue
    {
        get => _fieldValue;
        set => this.RaiseAndSetIfChanged(ref _fieldValue, value);
    }

    public override string DisplayValue => FieldValue?.ToString() ?? "";


    public BaseFieldGFFNodeViewModel(IGFFNode parent, string label) : base(parent)
    {
        Label = label;

        this.ObservableForProperty(x => x.Label).Subscribe(x => this.RaisePropertyChanged(nameof(Label)));
        this.ObservableForProperty(x => x.FieldValue).Subscribe(x => this.RaisePropertyChanged(nameof(DisplayValue)));
    }
}
