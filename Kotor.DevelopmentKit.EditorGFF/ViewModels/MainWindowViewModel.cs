using System;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using DynamicData.Binding;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IGFFTreeNodeViewModel _selectedNode;
        public IGFFTreeNodeViewModel SelectedNode
        {
            get => _selectedNode;
            set => this.RaiseAndSetIfChanged(ref _selectedNode, value);
        }

        private StructInListGFFTreeNodeViewModel _rootNode = new StructInListGFFTreeNodeViewModel(null);
        public StructInListGFFTreeNodeViewModel RootNode
        {
            get => _rootNode;
            set => this.RaiseAndSetIfChanged(ref _rootNode, value);
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
        public bool IsSelectedNodeStruct => _selectedNode is IStructGFFTreeNodeViewModel;
        public bool IsSelectedNodeList => _selectedNode is ListGFFTreeNodeViewModel;

        public HierarchicalTreeDataGridSource<IGFFTreeNodeViewModel> TreeData { get; }

        public MainWindowViewModel()
        {
            RootNode = new StructInListGFFTreeNodeViewModel(null);
            TreeData = new(RootNode)
            {
                Columns =
                {
                    new HierarchicalExpanderColumn<IGFFTreeNodeViewModel>(
                        new TextColumn<IGFFTreeNodeViewModel, string>("Label", x => x.Label, GridLength.Star), x => x.Children, isExpandedSelector: x => x.Expanded),
                    new TextColumn<IGFFTreeNodeViewModel, string>("Type", x => x.Type, GridLength.Parse("100")),
                    new TextColumn<IGFFTreeNodeViewModel, string>("Value", x => x.Value, GridLength.Parse("150")),
                },
            };

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
       
    }
}
