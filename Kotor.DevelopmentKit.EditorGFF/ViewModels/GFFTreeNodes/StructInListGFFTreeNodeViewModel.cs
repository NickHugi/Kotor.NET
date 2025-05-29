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

public class StructInListGFFTreeNodeViewModel : BaseGFFTreeNodeViewModel, BaseStructGFFTreeNodeViewModel
{
    public override string Label
    {
        get => $"[{(Parent as BaseGFFTreeNodeViewModel)!.Children.IndexOf(this).ToString()}]";
        set => throw new NotImplementedException();
    }
    public override bool CanEditLabel => false;
    public override string Type => "Struct";
    public override string Value => StructID.ToString();

    private int _structID;
    public int StructID
    {
        get => _structID;
        set => this.RaiseAndSetIfChanged(ref _structID, value);
    }

    private ObservableCollection<BaseGFFTreeNodeViewModel> _children = new([]);
    public override ReadOnlyObservableCollection<BaseGFFTreeNodeViewModel> Children => new(_children);


    public StructInListGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent) : base(parent)
    {
    }
    public StructInListGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, GFFStruct gffStruct) : base(parent)
    {
        //
    }

    public override void Delete()
    {
        if (Parent is ListGFFTreeNodeViewModel vmList)
        {
            vmList.DeleteStruct(this);
        }
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
