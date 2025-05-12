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

public class StructInListGFFTreeNodeViewModel : BaseStructGFFTreeNodeViewModel
{
    public override string Label
    {
        get => (Parent is null) ? "[Root]" : $"[{Parent.Children.IndexOf(this).ToString()}]";
        set => throw new NotImplementedException();
    }
    public override bool CanEditLabel => false;
    
    public StructInListGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent) : base(parent)
    {
    }
    public StructInListGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, GFFStruct gffStruct) : base(parent, gffStruct)
    {
    }

    public override void Delete()
    {
        if (Parent is ListGFFTreeNodeViewModel vmList)
        {
            vmList.DeleteStruct(this);
        }
    }
}
