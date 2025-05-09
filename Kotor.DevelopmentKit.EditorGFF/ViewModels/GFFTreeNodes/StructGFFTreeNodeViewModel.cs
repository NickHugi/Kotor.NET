using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public class StructGFFTreeNodeViewModel : BaseStructGFFTreeNodeViewModel, IFieldGFFTreeNodeViewModel
{
    private string _label = "";
    public override string Label
    {
        get => _label;
        set => this.RaiseAndSetIfChanged(ref _label, value);
    }

    public override bool CanEditLabel => true;

    public string Type => "Struct";
    public string Value => $"";

    public StructGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label) : base(parent)
    {
        Label = label;

        this.ObservableForProperty(x => x.Label).Subscribe(x => this.RaisePropertyChanged(nameof(Label)));
    }
    public StructGFFTreeNodeViewModel(IGFFTreeNodeViewModel parent, string label, int structID) : this(parent, label) 
    {
        StructID = structID;
    }

    public override void Delete()
    {
        ((BaseStructGFFTreeNodeViewModel)Parent).DeleteField(this);
    }
}

