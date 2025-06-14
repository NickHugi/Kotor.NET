using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Kotor.NET.Formats.BinaryGFF;
using Kotor.NET.Resources.KotorGFF;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class ListStructGFFNodeViewModel : BaseGFFNodeViewModel, IStructGFFTreeNodeViewModel
{
    public override string Label
    {
        get => $"[{(Parent as BaseGFFNodeViewModel)!.Children.IndexOf(this).ToString()}]";
        set => throw new NotImplementedException();
    }
    public override bool CanEditLabel => false;
    public override string DisplayType => "Struct";
    public override string DisplayValue => StructID.ToString();

    private int _structID;
    public int StructID
    {
        get => _structID;
        set => this.RaiseAndSetIfChanged(ref _structID, value);
    }

    private ObservableCollection<BaseGFFNodeViewModel> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFNodeViewModel> Children => new(_children);

    bool IStructGFFTreeNodeViewModel.IsDeleted => IsDeleted;

    public ListStructGFFNodeViewModel(IGFFNodeViewModel parent) : base(parent)
    {
    }
    public ListStructGFFNodeViewModel(IGFFNodeViewModel parent, GFFStruct gffStruct) : base(parent)
    {
        this.PopulateStruct(gffStruct);
    }
    public ListStructGFFNodeViewModel(IGFFNodeViewModel parent, int structID) : base(parent)
    {
        StructID = structID;
    }

    public override void Delete()
    {
        if (Parent is ListGFFNodeViewModel vmList)
        {
            vmList.DeleteStruct(this);
        }
    }

    public T AddField<T>(T field) where T : BaseFieldGFFNodeViewModel
    {
        _children.Add(field);
        Expanded = true;
        return field;
    }

    public void DeleteField(BaseFieldGFFNodeViewModel field)
    {
        _children.Remove(field);
    }

    public BaseFieldGFFNodeViewModel? GetField(string label)
    {
        return (BaseFieldGFFNodeViewModel?)Children.FirstOrDefault(x => x.Label == label);
    }
}
