using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Kotor.NET.Resources.KotorGFF;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class RootStructGFFTreeNodeViewModel : BaseGFFTreeNodeViewModel, BaseStructGFFTreeNodeViewModel
{
    private int _structID;
    public int StructID
    {
        get => _structID;
        set => this.RaiseAndSetIfChanged(ref _structID, value);
    }

    public override string Label
    {
        get => "";
        set => throw new NotSupportedException();
    }
    public override bool CanEditLabel => false;
    public override string Type => "Struct";
    public override string Value => StructID.ToString();

    private ObservableCollection<BaseGFFTreeNodeViewModel> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFTreeNodeViewModel> Children => new(_children);


    public RootStructGFFTreeNodeViewModel() : base(null)
    {
    }
    public RootStructGFFTreeNodeViewModel(GFFStruct gffStruct) : base(null)
    {
    }

    public override void Delete()
    {
        throw new InvalidOperationException("Cannot delete the root node of a GFF resource.");
    }

    public T AddField<T>(T field) where T : IFieldGFFTreeNodeViewModel
    {
        _children.Add(field);
        Expanded = true;
        return field;
    }

    public void DeleteField(IFieldGFFTreeNodeViewModel field)
    {
        _children.Remove(field);
    }

    public IFieldGFFTreeNodeViewModel? GetField(string label)
    {
        return (IFieldGFFTreeNodeViewModel?)Children.FirstOrDefault(x => x.Label == label);
    }
}
