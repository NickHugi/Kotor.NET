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
    public RootStructGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, GFFStruct gffStruct) : this(parent)
    {
        PopulateStruct(gffStruct);
    }

    public override void Delete()
    {
        throw new InvalidOperationException("Cannot delete the root node of a GFF resource.");
    }
}
