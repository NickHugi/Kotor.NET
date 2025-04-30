using System;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
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
        }
    }
}
