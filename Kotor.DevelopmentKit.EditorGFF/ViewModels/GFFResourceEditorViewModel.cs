using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Threading;
using DynamicData;
using DynamicData.Binding;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Actions;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.Binary2DA.Serialisation;
using Kotor.NET.Resources.KotorGFF;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels;

public class GFFResourceEditorViewModel : BaseResourceEditorViewModel<GFFViewModel, GFF>
{
    private BaseGFFNodeViewModel _selectedNode;
    public BaseGFFNodeViewModel SelectedNode
    {
        get => _selectedNode;
        set => this.RaiseAndSetIfChanged(ref _selectedNode, value);
    }

    private RootStructGFFNodeViewModel _rootNode = new RootStructGFFNodeViewModel();

    private HierarchicalTreeDataGridSource<BaseGFFNodeViewModel> _treeData;
    public HierarchicalTreeDataGridSource<BaseGFFNodeViewModel> TreeData
    {
        get => _treeData;
        set => this.RaiseAndSetIfChanged(ref _treeData, value);
    }

    private readonly ActionHistory<GFFResourceEditorViewModel> _history;
    public ActionHistory<GFFResourceEditorViewModel> History
    {
        get => _history;
    }

    public bool IsSelectedNodeUInt8 => _selectedNode is FieldUInt8GFFNodeViewModel;
    public bool IsSelectedNodeInt8 => _selectedNode is FieldInt8GFFNodeViewModel;
    public bool IsSelectedNodeUInt16 => _selectedNode is FieldUInt16GFFNodeViewModel;
    public bool IsSelectedNodeInt16 => _selectedNode is FieldInt16GFFNodeViewModel;
    public bool IsSelectedNodeUInt32 => _selectedNode is FieldUInt32GFFNodeViewModel;
    public bool IsSelectedNodeInt32 => _selectedNode is FieldInt32GFFNodeViewModel;
    public bool IsSelectedNodeUInt64 => _selectedNode is FieldUInt64GFFNodeViewModel;
    public bool IsSelectedNodeInt64 => _selectedNode is FieldInt64GFFNodeViewModel;
    public bool IsSelectedNodeSingle => _selectedNode is FieldSingleGFFNodeViewModel;
    public bool IsSelectedNodeDouble => _selectedNode is FieldDoubleGFFNodeViewModel;
    public bool IsSelectedNodeResRef => _selectedNode is FieldResRefGFFNodeViewModel;
    public bool IsSelectedNodeString => _selectedNode is FieldStringGFFNodeViewModel;
    public bool IsSelectedNodeLocalizedString => _selectedNode is FieldLocalizedStringGFFNodeViewModel;
    public bool IsSelectedNodeBinary => _selectedNode is FieldBinaryGFFNodeViewModel;
    public bool IsSelectedNodeVector3 => _selectedNode is FieldVector3GFFNodeViewModel;
    public bool IsSelectedNodeVector4 => _selectedNode is FieldVector4GFFNodeViewModel;
    public bool IsSelectedNodeStruct => _selectedNode is IStructGFFTreeNodeViewModel;
    public bool IsSelectedNodeList => _selectedNode is FieldListGFFNodeViewModel;


    public override string WindowTitlePrefix => throw new NotImplementedException();

    public GFFResourceEditorViewModel()
    {
        _history = new(this);
        Resource = new();

        this.WhenPropertyChanged(x => x.Resource.RootNode).Subscribe(x =>
        {
            CreateNewTree(Resource.RootNode);
        });

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

    private void CreateNewTree(RootStructGFFNodeViewModel rootNode)
    {
        _rootNode = rootNode;
        TreeData = new(_rootNode)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<BaseGFFNodeViewModel>(
                    new TextColumn<BaseGFFNodeViewModel, string>("Label", x => x.Label, GridLength.Star), x => x.Children, isExpandedSelector: x => x.Expanded),
                new TextColumn<BaseGFFNodeViewModel, string>("Type", x => x.Type, GridLength.Parse("100")),
                new TextColumn<BaseGFFNodeViewModel, string>("Value", x => x.Value, GridLength.Parse("150")),
            },
        };
    }

    public override void LoadModel(GFF model)
    {
        Resource.Load(model);
    }

    public override GFF BuildModel()
    {
        return Resource.Build();
    }
    public override GFF DeserializeModel(byte[] bytes)
    {
        return GFF.FromBytes(bytes);
    }
    public override GFF DeserializeModel(string path)
    {
        return GFF.FromFile(path);
    }
    public override byte[] SerializeModelToBytes()
    {
        var gff = BuildModel();
        using var memoryStream = new MemoryStream();
        new GFFBinarySerializer(gff).Serialize().Write(memoryStream);
        return memoryStream.ToArray();
    }
    public override void SerializeModelToFile()
    {
        var gff = BuildModel();
        using var fileStream = File.OpenWrite(FilePath);
        new GFFBinarySerializer(gff).Serialize().Write(fileStream);
    }

    public void DeleteNode(BaseGFFNodeViewModel node)
    {
        var action = new DeleteFieldAction(node);
        History.Apply(action);
    }

    public TTargetNode? NavigateTo<TTargetNode>(IEnumerable<object> path) where TTargetNode : BaseGFFNodeViewModel
    {
        IGFFNodeViewModel? node = _rootNode;

        foreach (var step in path)
        {
            if (step is int listIndex)
            {
                if (node is FieldListGFFNodeViewModel listNode)
                {
                    node = listNode.Children[listIndex];
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else if (step is string fieldLabel)
            {
                if (node is IStructGFFTreeNodeViewModel structNode)
                {
                    node = structNode.Children.OfType<BaseFieldGFFNodeViewModel>().FirstOrDefault(x => x.Label == fieldLabel);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        if (node is null)
        {
            return default;
        }
        else if (node is TTargetNode targetNode)
        {
            return targetNode;
        }
        else
        {
            throw new ArgumentException();
        }
    }
    public IEnumerable<object> GetPathOf(IGFFNodeViewModel node)
    {
        var next = node;
        var path = new List<object>();

        while (next is not null)
        {
            if (next is BaseFieldGFFNodeViewModel fieldNode)
            {
                path.Add(next.Label);
            }
            else if (next is ListStructGFFNodeViewModel structInList)
            {
                var listNode = (FieldListGFFNodeViewModel)structInList.Parent;
                path.Add(listNode.Children.IndexOf(structInList));
            }

            next = next.Parent;
        }

        path.Reverse();

        return path;
    }
    public BaseGFFNodeViewModel FillPath(IEnumerable<object> path)
    {
        BaseGFFNodeViewModel node = _rootNode;

        foreach (var step in path)
        {
            if (node is IStructGFFTreeNodeViewModel structNode && step is string fieldLabel)
            {
                var nextNode = structNode.GetField(fieldLabel);
                if (nextNode is null)
                {
                    structNode.AddField(new FieldStructGFFNodeViewModel(structNode as BaseGFFNodeViewModel, fieldLabel)); // TODO
                    node = structNode.GetField(fieldLabel);
                }
                else
                {
                    node = nextNode;
                }
            }
        }

        return node;
    }

    public void Undo()
    {
        History.Undo();
    }

    public void Redo()
    {
        History.Redo();
    }
}
