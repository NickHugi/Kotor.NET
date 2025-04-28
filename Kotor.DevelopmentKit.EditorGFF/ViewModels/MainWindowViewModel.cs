using System;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public StructInListGFFTreeNodeViewModel RootNode { get; }
        public HierarchicalTreeDataGridSource<IGFFTreeNodeViewModel> TreeData { get; }

        public MainWindowViewModel()
        {
            RootNode = new StructInListGFFTreeNodeViewModel(null);
            TreeData = new(RootNode)
            {
                Columns =
                {
                    new HierarchicalExpanderColumn<IGFFTreeNodeViewModel>(
                        new TextColumn<IGFFTreeNodeViewModel, string>("Label", x => x.Name, GridLength.Star), x => x.Children, isExpandedSelector: x => x.Expanded),
                    new TextColumn<IGFFTreeNodeViewModel, string>("Type", x => x.Type, GridLength.Parse("100")),
                    new TextColumn<IGFFTreeNodeViewModel, string>("Value", x => x.Value, GridLength.Parse("150")),
                },
            };
        }
    }
}
