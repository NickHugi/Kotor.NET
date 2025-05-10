using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using DynamicData.Binding;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels;

public class MainWindowViewModel : BaseResourceEditorViewModel<RootStructGFFTreeNodeViewModel, GFF>
{
    private IGFFTreeNodeViewModel _selectedNode;
    public IGFFTreeNodeViewModel SelectedNode
    {
        get => _selectedNode;
        set => this.RaiseAndSetIfChanged(ref _selectedNode, value);
    }

    private RootStructGFFTreeNodeViewModel _rootNode = new RootStructGFFTreeNodeViewModel(null);

    private HierarchicalTreeDataGridSource<IGFFTreeNodeViewModel> _treeData;
    public HierarchicalTreeDataGridSource<IGFFTreeNodeViewModel> TreeData
    {
        get => _treeData;
        set => this.RaiseAndSetIfChanged(ref _treeData, value);
    }

    public bool IsSelectedNodeUInt8 => _selectedNode is UInt8GFFTreeNodeViewModel;
    public bool IsSelectedNodeInt8 => _selectedNode is Int8GFFTreeNodeViewModel;
    public bool IsSelectedNodeUInt16 => _selectedNode is UInt16GFFTreeNodeViewModel;
    public bool IsSelectedNodeInt16 => _selectedNode is Int16GFFTreeNodeViewModel;
    public bool IsSelectedNodeUInt32 => _selectedNode is UInt32GFFTreeNodeViewModel;
    public bool IsSelectedNodeInt32 => _selectedNode is Int32GFFTreeNodeViewModel;
    public bool IsSelectedNodeUInt64 => _selectedNode is UInt64GFFTreeNodeViewModel;
    public bool IsSelectedNodeInt64 => _selectedNode is Int64GFFTreeNodeViewModel;
    public bool IsSelectedNodeSingle => _selectedNode is SingleGFFTreeNodeViewModel;
    public bool IsSelectedNodeDouble => _selectedNode is DoubleGFFTreeNodeViewModel;
    public bool IsSelectedNodeResRef => _selectedNode is ResRefGFFTreeNodeViewModel;
    public bool IsSelectedNodeString => _selectedNode is StringGFFTreeNodeViewModel;
    public bool IsSelectedNodeLocalizedString => _selectedNode is LocalizedStringGFFTreeNodeViewModel;
    public bool IsSelectedNodeBinary => _selectedNode is BinaryGFFTreeNodeViewModel;
    public bool IsSelectedNodeVector3 => _selectedNode is Vector3GFFTreeNodeViewModel;
    public bool IsSelectedNodeVector4 => _selectedNode is Vector4GFFTreeNodeViewModel;
    public bool IsSelectedNodeStruct => _selectedNode is BaseStructGFFTreeNodeViewModel;
    public bool IsSelectedNodeList => _selectedNode is ListGFFTreeNodeViewModel;


    public override string WindowTitlePrefix => throw new NotImplementedException();

    public MainWindowViewModel()
    {
        CreateNewTree(new());

        this.WhenPropertyChanged(x => x.SelectedNode).Subscribe(x =>
        {
            this.RaisePropertyChanged(nameof(IsSelectedNodeUInt8));
            this.RaisePropertyChanged(nameof(IsSelectedNodeInt8));
            this.RaisePropertyChanged(nameof(IsSelectedNodeUInt16));
            this.RaisePropertyChanged(nameof(IsSelectedNodeUInt16));
            this.RaisePropertyChanged(nameof(IsSelectedNodeUInt32));
            this.RaisePropertyChanged(nameof(IsSelectedNodeUInt32));
            this.RaisePropertyChanged(nameof(IsSelectedNodeUInt64));
            this.RaisePropertyChanged(nameof(IsSelectedNodeUInt64));
            this.RaisePropertyChanged(nameof(IsSelectedNodeSingle));
            this.RaisePropertyChanged(nameof(IsSelectedNodeDouble));
            this.RaisePropertyChanged(nameof(IsSelectedNodeResRef));
            this.RaisePropertyChanged(nameof(IsSelectedNodeString));
            this.RaisePropertyChanged(nameof(IsSelectedNodeLocalizedString));
            this.RaisePropertyChanged(nameof(IsSelectedNodeBinary));
            this.RaisePropertyChanged(nameof(IsSelectedNodeVector3));
            this.RaisePropertyChanged(nameof(IsSelectedNodeVector4));
            this.RaisePropertyChanged(nameof(IsSelectedNodeStruct));
            this.RaisePropertyChanged(nameof(IsSelectedNodeList));
        });
    }

    private void CreateNewTree(RootStructGFFTreeNodeViewModel rootNode)
    {
        _rootNode = rootNode;
        TreeData = new(_rootNode)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<IGFFTreeNodeViewModel>(
                    new TextColumn<IGFFTreeNodeViewModel, string>("Label", x => x.Label, GridLength.Star), x => x.Children, isExpandedSelector: x => x.Expanded),
                new TextColumn<IGFFTreeNodeViewModel, string>("Type", x => x.Type, GridLength.Parse("100")),
                new TextColumn<IGFFTreeNodeViewModel, string>("Value", x => x.Value, GridLength.Parse("150")),
            },
        };
    }

    public override void LoadModel(GFF model)
    {
        CreateNewTree(new(model.Root));
    }

    public override GFF BuildModel() => throw new NotImplementedException();
    public override GFF DeserializeModel(byte[] bytes)
    {
        return GFF.FromBytes(bytes);
    }
    public override GFF DeserializeModel(string path)
    {
        return GFF.FromFile(path);
    }
    public override byte[] SerializeModelToBytes() => throw new NotImplementedException();
    public override void SerializeModelToFile() => throw new NotImplementedException();
}
