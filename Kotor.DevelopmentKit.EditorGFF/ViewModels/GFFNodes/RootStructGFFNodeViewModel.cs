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

public class RootStructGFFNodeViewModel : BaseGFFNodeViewModel, IStructGFFTreeNodeViewModel
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

    private ObservableCollection<BaseGFFNodeViewModel> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFNodeViewModel> Children => new(_children);


    public RootStructGFFNodeViewModel() : base(null)
    {
    }
    public RootStructGFFNodeViewModel(GFFStruct gffStruct) : base(null)
    {
    }

    public override void Delete()
    {
        throw new InvalidOperationException("Cannot delete the root node of a GFF resource.");
    }

    public T AddField<T>(T field) where T : IFieldGFFNodeViewModel
    {
        _children.Add(field);
        Expanded = true;
        return field;
    }

    public void DeleteField(IFieldGFFNodeViewModel field)
    {
        _children.Remove(field);
    }

    public IFieldGFFNodeViewModel? GetField(string label)
    {
        return (IFieldGFFNodeViewModel?)Children.FirstOrDefault(x => x.Label == label);
    }
}
