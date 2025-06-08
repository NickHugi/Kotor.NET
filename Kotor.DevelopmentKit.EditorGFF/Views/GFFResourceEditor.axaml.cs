using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.Views;
using Kotor.DevelopmentKit.EditorGFF.Actions;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.Services;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.Kotor2DA;
using Kotor.NET.Resources.KotorGFF;
using Microsoft.VisualBasic;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class MainWindow : ResourceEditorBase<GFFResourceEditorViewModel, GFFViewModel, GFF>
{
    public override FilePickerFileType AllValidFilePickerFileTypes => new FilePickerFileType("All Valid Options")
    {
        Patterns = [.. FilePickerTypes.GFF.Patterns!, .. FilePickerTypes.Encapsulated.Patterns!],
    };
    public override FilePickerOpenOptions FilePickerOpenOptions => new()
    {
        Title = "Open GFF File",
        AllowMultiple = false,
        FileTypeFilter = [FilePickerTypes.GFF, FilePickerTypes.Encapsulated, AllValidFilePickerFileTypes, FilePickerTypes.All],
    };
    public override FilePickerSaveOptions FilePickerSaveOptions => new()
    {
        Title = "Save GFF File",
        ShowOverwritePrompt = false,
        FileTypeChoices = [FilePickerTypes.GFF, FilePickerTypes.Encapsulated, AllValidFilePickerFileTypes, FilePickerTypes.All],
    };
    public override List<ResourceType> ResourceTypes => [ResourceType.GFF];


    public MainWindow()
    {
        InitializeComponent();
    }
    
    private async void TreeDataGrid_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed)
        {
            var position = e.GetPosition(TreeDataGrid);
            var visual = (Visual?)TreeDataGrid.InputHitTest(position);
            var row = visual?.FindAncestorOfType<TreeDataGridRow>();
            var data = (BaseGFFNodeViewModel)TreeDataGrid?.RowSelection?.SelectedItem!;

            if (row != null)
            {
                var menu = await GetStructContextMenu(data);

                menu.Open(row);
            }
        }
    }
    private void FieldUInt8Panel_FinishedEditing(object? sender, UInt8EditedEventArgs e)
    {
        Context.SetUInt8Node(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldInt8Panel_FinishedEditing(object? sender, Int8EditedEventArgs e)
    {
        Context.SetInt8Node(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldUInt16Panel_FinishedEditing(object? sender, UInt16EditedEventArgs e)
    {
        Context.SetUInt16Node(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldInt16Panel_FinishedEditing(object? sender, Int16EditedEventArgs e)
    {
        Context.SetInt16Node(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldUInt32Panel_FinishedEditing(object? sender, UInt32EditedEventArgs e)
    {
        Context.SetUInt32Node(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldInt32Panel_FinishedEditing(object? sender, Int32EditedEventArgs e)
    {
        Context.SetInt32Node(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldUInt64Panel_FinishedEditing(object? sender, UInt64EditedEventArgs e)
    {
        Context.SetUInt64Node(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldInt64Panel_FinishedEditing(object? sender, Int64EditedEventArgs e)
    {
        Context.SetInt64Node(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldSinglePanel_FinishedEditing(object? sender, SingleEditedEventArgs e)
    {
        Context.SetSingleNode(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldDoublePanel_FinishedEditing(object? sender, DoubleEditedEventArgs e)
    {
        Context.SetDoubleNode(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldStringPanel_FinishedEditing(object? sender, StringEditedEventArgs e)
    {
        Context.SetStringNode(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldResRefPanel_FinishedEditing(object? sender, ResRefEditedEventArgs e)
    {
        Context.SetResRefNode(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldLocalizedStringPanel_FinishedEditing(object? sender, LocalizedStringEditedEventArgs e)
    {
        Context.SetLocalizedStringNode(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldBinaryPanel_FinishedEditing(object? sender, BinaryEditedEventArgs e)
    {
        Context.SetBinaryNode(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldVector3Panel_FinishedEditing(object? sender, Vector3EditedEventArgs e)
    {
        Context.SetVector3Node(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldVector4Panel_FinishedEditing(object? sender, Vector4EditedEventArgs e)
    {
        Context.SetVector4Node(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void FieldStructPanel_FinishedEditing(object? sender, StructEditedEventArgs e)
    {
        Context.SetStructNode(Context.RootNode.GetPathOf(e.EditedNode), e.NewValue);
    }
    private void LabelInput_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var action = new SetFieldLabel(Context.RootNode.GetPathOf(Context.SelectedNode), ((BaseFieldGFFNodeViewModel)Context.SelectedNode).Label, ((TextBox)sender).Text);
        Context.History.Apply(action);
    }

    private async Task<ContextMenu> GetStructContextMenu(BaseGFFNodeViewModel node)
    {
        var menu = new ContextMenu();

        if (node is IStructGFFTreeNodeViewModel dataAsStruct)
        {
            menu.Items.Add(new MenuItem() { Header = "Add UInt8", Command = ReactiveCommand.Create(() => AddUInt8(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add Int8", Command = ReactiveCommand.Create(() => AddInt8(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add UInt16", Command = ReactiveCommand.Create(() => AddUInt16(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add Int16", Command = ReactiveCommand.Create(() => AddInt16(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add UInt32", Command = ReactiveCommand.Create(() => AddUInt32(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add Int32", Command = ReactiveCommand.Create(() => AddInt32(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add UInt64", Command = ReactiveCommand.Create(() => AddUInt64(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add Int64", Command = ReactiveCommand.Create(() => AddInt64(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add Single", Command = ReactiveCommand.Create(() => AddSingle(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add Double", Command = ReactiveCommand.Create(() => AddDouble(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add ResRef", Command = ReactiveCommand.Create(() => AddResRef(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add String", Command = ReactiveCommand.Create(() => AddString(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add Localized String", Command = ReactiveCommand.Create(() => AddLocalizedString(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add Binary", Command = ReactiveCommand.Create(() => AddBinary(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add Vector3", Command = ReactiveCommand.Create(() => AddVector3(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add Vector4", Command = ReactiveCommand.Create(() => AddVector4(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add Struct", Command = ReactiveCommand.Create(() => AddStruct(dataAsStruct)) });
            menu.Items.Add(new MenuItem() { Header = "Add List", Command = ReactiveCommand.Create(() => AddList(dataAsStruct)) });
        }
        else if (node is FieldListGFFNodeViewModel dataAsList)
        {
            menu.Items.Add(new MenuItem() { Header = "Add Struct", Command = ReactiveCommand.Create(() => AddStruct(dataAsList)) });
        }

        if (menu.Items.Count > 0)
        {
            menu.Items.Add(new Separator());
        }

        if (node is FieldListGFFNodeViewModel)
        {
            menu.Items.Add(new MenuItem() { Header = "Paste Struct", Command = ReactiveCommand.Create(() => PasteNode()), IsEnabled=await HasStructOnClipboard() });
            menu.Items.Add(new MenuItem() { Header = "Copy Field", Command = ReactiveCommand.Create(async () => await CopyNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Cut Field", Command = ReactiveCommand.Create(async() => await CutNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Delete Field", Command = ReactiveCommand.Create(() => DeleteNode(node)) });
        }
        else if (node is RootStructGFFNodeViewModel)
        {
            menu.Items.Add(new MenuItem() { Header = "Paste Field", Command = ReactiveCommand.Create(() => PasteNode()), IsEnabled = await HasFieldOnClipboard() });
        }
        else if (node is ListStructGFFNodeViewModel)
        {
            menu.Items.Add(new MenuItem() { Header = "Paste Field", Command = ReactiveCommand.Create(() => PasteNode()), IsEnabled = await HasFieldOnClipboard() });
            menu.Items.Add(new MenuItem() { Header = "Copy Struct", Command = ReactiveCommand.Create(async () => await CopyNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Cut Struct", Command = ReactiveCommand.Create(async () => await CutNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Delete Struct", Command = ReactiveCommand.Create(() => DeleteNode(node)) });
        }
        else if (node is FieldStructGFFNodeViewModel)
        {
            menu.Items.Add(new MenuItem() { Header = "Paste Field", Command = ReactiveCommand.Create(() => PasteNode()), IsEnabled = await HasFieldOnClipboard() });
            menu.Items.Add(new MenuItem() { Header = "Copy Field", Command = ReactiveCommand.Create(async () => await CopyNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Cut Field", Command = ReactiveCommand.Create(async () => await CutNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Delete Field", Command = ReactiveCommand.Create(() => DeleteNode(node)) });
        }
        else if (node is BaseFieldGFFNodeViewModel)
        {
            menu.Items.Add(new MenuItem() { Header = "Copy Field", Command = ReactiveCommand.Create(async () => await CopyNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Cut Field", Command = ReactiveCommand.Create(async () => await CutNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Delete Field", Command = ReactiveCommand.Create(() => DeleteNode(node)) });
        }

        return menu;
    }

    private void AddUInt8(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New UInt8");
        Context.SetUInt8Node(path, default);
    }
    private void AddInt8(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New Int8");
        Context.SetInt8Node(path, default);
    }
    private void AddUInt16(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New UInt16");
        Context.SetUInt16Node(path, default);
    }
    private void AddInt16(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New Int16");
        Context.SetInt16Node(path, default);
    }
    private void AddUInt32(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New UInt32");
        Context.SetUInt32Node(path, default);
    }
    private void AddInt32(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New Int32");
        Context.SetInt32Node(path, default);
    }
    private void AddUInt64(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New UInt64");
        Context.SetUInt64Node(path, default);
    }
    private void AddInt64(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New UInt64");
        Context.SetUInt64Node(path, default);
    }
    private void AddSingle(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New Single");
        Context.SetSingleNode(path, default);
    }
    private void AddDouble(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New Double");
        Context.SetDoubleNode(path, default);
    }
    private void AddResRef(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New ResRef");
        Context.SetResRefNode(path, new());
    }
    private void AddString(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New String");
        Context.SetStringNode(path, "");
    }
    private void AddLocalizedString(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New Localized String");
        Context.SetLocalizedStringNode(path, new());
    }
    private void AddBinary(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New Single");
        Context.SetSingleNode(path, default);
    }
    private void AddVector3(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New Vector3");
        Context.SetVector3Node(path, default);
    }
    private void AddVector4(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New Vector4");
        Context.SetVector4Node(path, default);
    }
    private void AddStruct(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New Struct");
        Context.SetStructNode(path, default);
    }
    private void AddStruct(FieldListGFFNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend(parent.Children.Count);
        Context.SetStructNode(path, default);
    }
    private void AddList(IStructGFFTreeNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend("New List");
        Context.SetList(path);
    }

    private async Task CutNode(BaseGFFNodeViewModel node)
    {
        await CopyNode(node);
        DeleteNode(node);
    }

    private void DeleteNode(BaseGFFNodeViewModel node)
    {
        Context.DeleteNode(node);
    }

    private async Task CopyNode(BaseGFFNodeViewModel node)
    {
        var serilizeService = new SerializeNodeService();
        var text = serilizeService.Serialize(node);
        await Clipboard!.SetTextAsync(text);
    }

    private async Task PasteNode()
    {
        var deserializeNodeService = new DeserializeNodeService();
        var selectedNode = Context.SelectedNode;

        if (await HasStructOnClipboard() && selectedNode is FieldListGFFNodeViewModel selectedListNode)
        {
            var text = await Clipboard!.GetTextAsync();
            var node = (ListStructGFFNodeViewModel)deserializeNodeService.Deserialize(selectedNode, text);
            selectedListNode.AddStruct(node);
        }
        else if (await HasFieldOnClipboard() && selectedNode is IStructGFFTreeNodeViewModel selectedStructNode)
        {
            var text = await Clipboard!.GetTextAsync();
            var node = (BaseFieldGFFNodeViewModel)deserializeNodeService.Deserialize(selectedNode, text);
            selectedStructNode.AddField(node);
        }
        else
        {
            throw new NotSupportedException();
        }
    }

    private async Task<bool> HasStructOnClipboard()
    {
        try
        {
            var clipboard = await Clipboard!.GetTextAsync();
            var document = XDocument.Parse(clipboard);
            return document.Elements().FirstOrDefault()?.Name?.ToString() == "Struct";
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> HasFieldOnClipboard()
    {
        try
        {
            var clipboard = await Clipboard!.GetTextAsync();
            var document = XDocument.Parse(clipboard);
            return document.Elements().FirstOrDefault()?.Name?.ToString() == "Field";
        }
        catch
        {
            return false;
        }
    }

    public void Undo()
    {
        Dispatcher.UIThread.Post(() => Context.Undo(), DispatcherPriority.Default);
    }

    public void Redo()
    {
        Dispatcher.UIThread.Post(() => Context.Redo(), DispatcherPriority.Default);
    }
}
