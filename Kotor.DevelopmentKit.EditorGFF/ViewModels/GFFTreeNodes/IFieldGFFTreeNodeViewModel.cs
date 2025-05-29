using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public abstract class IFieldGFFTreeNodeViewModel : BaseGFFTreeNodeViewModel
{
    public override string Value => "";
    public override bool CanEditLabel => true;

    private string _label = "";
    public override string Label
    {
        get => _label;
        set => this.RaiseAndSetIfChanged(ref _label, value);
    }

    private ReadOnlyObservableCollection<BaseGFFTreeNodeViewModel> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFTreeNodeViewModel> Children => _children;

    protected IFieldGFFTreeNodeViewModel(IGFFTreeNodeViewModel? parent) : base(parent)
    {
    }
}

public abstract class IFieldGFFTreeNodeViewModel<T> : IFieldGFFTreeNodeViewModel where T  :notnull
{
    private T _fieldValue = default!;
    public T FieldValue
    {
        get => _fieldValue;
        set => this.RaiseAndSetIfChanged(ref _fieldValue, value);
    }

    public override string Value => FieldValue?.ToString() ?? "";


    public IFieldGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label) : base(parent)
    {
        Label = label;

        this.ObservableForProperty(x => x.Label).Subscribe(x => this.RaisePropertyChanged(nameof(Label)));
        this.ObservableForProperty(x => x.FieldValue).Subscribe(x => this.RaisePropertyChanged(nameof(Value)));
    }

    public override void Delete()
    {
        if (Parent is BaseStructGFFTreeNodeViewModel parentStruct)
        {
            parentStruct.DeleteField(this);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}
