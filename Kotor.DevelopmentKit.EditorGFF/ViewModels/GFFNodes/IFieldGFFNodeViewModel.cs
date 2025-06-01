using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public abstract class IFieldGFFNodeViewModel : BaseGFFNodeViewModel
{
    public override string Value => "";
    public override bool CanEditLabel => true;

    private string _label = "";
    public override string Label
    {
        get => _label;
        set => this.RaiseAndSetIfChanged(ref _label, value);
    }

    private ReadOnlyObservableCollection<BaseGFFNodeViewModel> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFNodeViewModel> Children => _children;

    protected IFieldGFFNodeViewModel(IGFFNodeViewModel? parent) : base(parent)
    {
    }
}

public abstract class IFieldGFFTreeNodeViewModel<T> : IFieldGFFNodeViewModel where T  :notnull
{
    private T _fieldValue = default!;
    public T FieldValue
    {
        get => _fieldValue;
        set => this.RaiseAndSetIfChanged(ref _fieldValue, value);
    }

    public override string Value => FieldValue?.ToString() ?? "";


    public IFieldGFFTreeNodeViewModel(IGFFNodeViewModel parent, string label) : base(parent)
    {
        Label = label;

        this.ObservableForProperty(x => x.Label).Subscribe(x => this.RaisePropertyChanged(nameof(Label)));
        this.ObservableForProperty(x => x.FieldValue).Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
    }

    public override void Delete()
    {
        if (Parent is IStructGFFTreeNodeViewModel parentStruct)
        {
            parentStruct.DeleteField(this);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}
