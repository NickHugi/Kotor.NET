using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using DynamicData;
using DynamicData.Binding;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Actions;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;
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
    public RootStructGFFNodeViewModel RootNode => _rootNode;

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
                new TextColumn<BaseGFFNodeViewModel, string>("Type", x => x.DisplayType, GridLength.Parse("100")),
                new TextColumn<BaseGFFNodeViewModel, string>("Value", x => x.DisplayValue, GridLength.Parse("150")),
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

    public void SetUInt8Node(NodePath path, byte value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetUInt8Action(path, oldValue, value));
    }

    public void SetInt8Node(NodePath path, sbyte value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetInt8Action(path, oldValue, value));
    }

    public void SetUInt16Node(NodePath path, UInt16 value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetUInt16Action(path, oldValue, value));
    }

    public void SetInt16Node(NodePath path, Int16 value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetInt16Action(path, oldValue, value));
    }

    public void SetUInt32Node(NodePath path, UInt32 value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetUInt32Action(path, oldValue, value));
    }

    public void SetInt32Node(NodePath path, Int32 value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetInt32Action(path, oldValue, value));
    }

    public void SetUInt64Node(NodePath path, UInt64 value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetUInt64Action(path, oldValue, value));
    }

    public void SetInt64Node(NodePath path, Int64 value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetInt64Action(path, oldValue, value));
    }

    public void SetSingleNode(NodePath path, Single value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetSingleAction(path, oldValue, value));
    }

    public void SetDoubleNode(NodePath path, Double value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetDoubleAction(path, oldValue, value));
    }

    public void SetResRefNode(NodePath path, ResRefViewModel value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetResRefAction(path, oldValue, value));
    }

    public void SetStringNode(NodePath path, String value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetStringAction(path, oldValue, value));
    }

    public void SetLocalizedStringNode(NodePath path, LocalizedStringViewModel value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetLocalizedStringAction(path, oldValue, value));
    }

    public void SetBinaryNode(NodePath path, byte[] value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetBinaryAction(path, oldValue, value));
    }

    public void SetVector3Node(NodePath path, Vector3ViewModel value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetVector3Action(path, oldValue, value));
    }

    public void SetVector4Node(NodePath path, Vector4ViewModel value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetVector4Action(path, oldValue, value));
    }

    public void SetList(NodePath path)
    {
        var (existing, missing) = RootNode.SplitPath(new NodePath(path.SkipLast(1)));
        if (missing.Count() > 1)
        {
            var fillAction = new CreatePathAction(existing, missing);
            History.Apply(fillAction);
        }

        var node = RootNode.NavigateTo<ListStructGFFNodeViewModel>(path);

        var setAction = new SetListAction(path);
        History.Apply(setAction);
    }

    public void SetStructNode(NodePath path, Int32 value)
    {
        var (existing, missing) = RootNode.SplitPath(new NodePath(path.SkipLast(1)));
        if (missing.Count() > 1)
        {
            var fillAction = new CreatePathAction(existing, missing);
            History.Apply(fillAction);
        }

        var node = RootNode.NavigateTo<BaseGFFNodeViewModel>(path) as IStructGFFTreeNodeViewModel;

        var oldValue = node?.StructID;
        if (value.Equals(oldValue))
            return;

        var setAction = new SetStructAction(path, oldValue, value);
        History.Apply(setAction);
    }

    private void SetFieldValueForNode<TValue>(NodePath path, TValue newValue, Func<TValue?, IAction<GFFResourceEditorViewModel>> createAction)
        where TValue : struct
    {
        var (existing, missing) = RootNode.SplitPath(new NodePath(path.SkipLast(1)));
        if (missing.Count() > 1)
        {
            var fillAction = new CreatePathAction(existing, missing);
            History.Apply(fillAction);
        }

        var node = RootNode.NavigateTo<BaseFieldGFFNodeViewModel<TValue>>(path);
        TValue? oldValue = (node is null) ? null : node.FieldValue;

        if (newValue.Equals(oldValue))
            return;

        var setAction = createAction(oldValue);
        History.Apply(setAction);
    }
    private void SetFieldValueForNode<TValue>(NodePath path, TValue newValue, Func<TValue?, IAction<GFFResourceEditorViewModel>> createAction)
        where TValue : class
    {
        var (existing, missing) = RootNode.SplitPath(new NodePath(path.SkipLast(1)));
        if (missing.Count() > 1)
        {
            var fillAction = new CreatePathAction(existing, missing);
            History.Apply(fillAction);
        }

        var node = RootNode.NavigateTo<BaseFieldGFFNodeViewModel<TValue>>(path);
        TValue? oldValue = (node is null) ? default(TValue?) : node.FieldValue;

        if (newValue.Equals(oldValue))
            return;

        var setAction = createAction(oldValue);
        History.Apply(setAction);
    }

    public void Undo()
    {
        History.Undo();
    }

    public void Redo()
    {
        History.Redo();
    }

    private string FindOriginalFieldLabel(IStructGFFTreeNodeViewModel structNode, string label)
    {
        if (structNode.GetField(label) is null)
            return label;

        int count = 1;
        var newLabel = label;

        do
        {
            count++;

            var suffix = "(" + count + ")";
            label = label + suffix;
        } while (structNode.GetField(newLabel) is not null);

        return label;
    }
}
