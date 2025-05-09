using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class RootStructGFFTreeNodeViewModel : BaseStructGFFTreeNodeViewModel
{
    public override string Label
    {
        get => (Parent is null) ? "[Root]" : $"[{Parent.Children.IndexOf(this).ToString()}]";
        set => throw new NotImplementedException();
    }
    public override bool CanEditLabel => false;

    public RootStructGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent) : base(parent)
    {
    }

    public override void Delete()
    {
        ((ListGFFTreeNodeViewModel)Parent).DeleteStruct(this);
    }
}
