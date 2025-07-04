using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using DynamicData;
using DynamicData.Binding;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using Kotor.DevelopmentKit.Base.Settings.Values;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Actions;
using Kotor.DevelopmentKit.EditorGFF.Extensions;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.Settings;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;
using Kotor.NET.Formats.Binary2DA.Serialisation;
using Kotor.NET.Resources.KotorGFF;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels;

public class GFFResourceEditorViewModel : BaseResourceEditorViewModel<GFFViewModel, GFF>
{
    private BaseGFFNode? _selectedNode;
    public BaseGFFNode? SelectedNode
    {
        get => _selectedNode;
        set => this.RaiseAndSetIfChanged(ref _selectedNode, value);
    }

    private RootStructGFFNode _rootNode = new RootStructGFFNode();
    public RootStructGFFNode RootNode => _rootNode;

    private HierarchicalTreeDataGridSource<BaseGFFNode> _treeData;
    public HierarchicalTreeDataGridSource<BaseGFFNode> TreeData
    {
        get => _treeData;
        set => this.RaiseAndSetIfChanged(ref _treeData, value);
    }

    private readonly ActionHistory<GFFResourceEditorViewModel> _history;
    public ActionHistory<GFFResourceEditorViewModel> History
    {
        get => _history;
    }

    public INodePanelViewModel? PanelData
    {
        get
        {
            if (_selectedNode is null)
                return null;

            var path = RootNode.GetPathOf(_selectedNode);

            return _selectedNode switch
            {
                UInt8GFFNode node => ActivatorUtilities.CreateInstance<UInt8PanelViewModel>(App.ServiceProvider, node.FieldValue),
                Int8GFFNode node => ActivatorUtilities.CreateInstance<Int8PanelViewModel>(App.ServiceProvider, node.FieldValue),
                UInt16GFFNode node => ActivatorUtilities.CreateInstance<UInt16PanelViewModel>(App.ServiceProvider, node.FieldValue),
                Int16GFFNode node => ActivatorUtilities.CreateInstance<Int16PanelViewModel>(App.ServiceProvider, node.FieldValue),
                UInt32GFFNode node => ActivatorUtilities.CreateInstance<UInt32PanelViewModel>(App.ServiceProvider, node.FieldValue),
                Int32GFFNode node => ActivatorUtilities.CreateInstance<Int32PanelViewModel>(App.ServiceProvider, node.FieldValue),
                UInt64GFFNode node => ActivatorUtilities.CreateInstance<UInt64PanelViewModel>(App.ServiceProvider, node.FieldValue),
                Int64GFFNode node => ActivatorUtilities.CreateInstance<Int64PanelViewModel>(App.ServiceProvider, node.FieldValue),
                SingleGFFNode node => ActivatorUtilities.CreateInstance<SinglePanelViewModel>(App.ServiceProvider, node.FieldValue),
                DoubleGFFNode node => ActivatorUtilities.CreateInstance<DoublePanelViewModel>(App.ServiceProvider, node.FieldValue),
                ResRefGFFNode node => ActivatorUtilities.CreateInstance<ResRefPanelViewModel>(App.ServiceProvider, node.FieldValue),
                StringGFFNode node => ActivatorUtilities.CreateInstance<StringPanelViewModel>(App.ServiceProvider, node.FieldValue),
                LocalizedStringGFFNode node => ActivatorUtilities.CreateInstance<LocalizedStringPanelViewModel>(App.ServiceProvider, node.FieldValue),
                BinaryGFFNode node => ActivatorUtilities.CreateInstance<BinaryPanelViewModel>(App.ServiceProvider, node.FieldValue),
                IStructGFFNode node => ActivatorUtilities.CreateInstance<StructPanelViewModel>(App.ServiceProvider, node.StructID),
                Vector3GFFNode node => ActivatorUtilities.CreateInstance<Vector3PanelViewModel>(App.ServiceProvider, node.FieldValue),
                Vector4GFFNode node => ActivatorUtilities.CreateInstance<Vector4PanelViewModel>(App.ServiceProvider, node.FieldValue),
                ListGFFNode node => null,
                _ => throw new InvalidOperationException()
            };
        }
    }

    public override string WindowTitlePrefix => "GFF Editor";


    public GFFResourceEditorViewModel(DefaultSettingsRoot settings) : base(settings)
    {
        _treeData = new(new RootStructGFFNode());
        _history = new(this);
        Resource = new();

        this.WhenPropertyChanged(x => x.Resource.RootNode).Subscribe(x =>
        {
            CreateNewTree(Resource.RootNode);
        });

        this.WhenPropertyChanged(x => x.SelectedNode).Subscribe(x =>
        {
            this.RaisePropertyChanged(nameof(PanelData));
        });
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

    public void DeleteNode(BaseGFFNode node)
    {
        var action = new DeleteFieldAction(node);
        History.Apply(action);
    }

    public BaseGFFNode FillPath(NodePath path)
    {
        BaseGFFNode node = _rootNode;

        foreach (var step in path)
        {
            if (node is IStructGFFNode structNode && step is string fieldLabel)
            {
                var nextNode = structNode.GetField(fieldLabel);
                if (nextNode is null)
                {
                    var field = structNode as BaseGFFNode;
                    structNode.AddField(new FieldStructGFFNode(field, fieldLabel));
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

    public void SetResRefNode(NodePath path, ReactiveResRef value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetResRefAction(path, oldValue, value));
    }

    public void SetStringNode(NodePath path, String value)
    {
        SetFieldValueForNode(path, value, (oldValue) => new SetStringAction(path, oldValue, value));
    }

    public void SetLocalizedStringNode(NodePath path, ReactiveLocalizedString value)
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

        var node = RootNode.NavigateTo<ListStructGFFNode>(path);

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

        var node = RootNode.NavigateTo<BaseGFFNode>(path) as IStructGFFNode;

        var oldValue = node?.StructID;
        if (value.Equals(oldValue))
            return;

        var setAction = new SetStructAction(path, oldValue, value);
        History.Apply(setAction);
    }

    public void Relabel(NodePath path, string label)
    {
        var node = RootNode.NavigateTo<BaseGFFNode>(path);
        var action = new SetFieldLabel(path, node.Label, label);
        History.Apply(action);
    }

    public void Undo()
    {
        History.Undo();
    }

    public void Redo()
    {
        History.Redo();
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

    private void CreateNewTree(RootStructGFFNode rootNode)
    {
        _rootNode = rootNode;
        TreeData = new(_rootNode)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<BaseGFFNode>(
                    new TextColumn<BaseGFFNode, string>("Label", x => x.Label, GridLength.Star), x => x.Children, isExpandedSelector: x => x.Expanded),
                new TextColumn<BaseGFFNode, string>("Type", x => x.DisplayType, GridLength.Parse("100")),
                new TextColumn<BaseGFFNode, string>("Value", x => x.DisplayValue, GridLength.Parse("150")),
            },
        };
    }
}
