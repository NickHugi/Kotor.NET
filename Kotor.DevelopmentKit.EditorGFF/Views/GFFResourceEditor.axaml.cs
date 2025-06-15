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
using Kotor.DevelopmentKit.EditorGFF.Extensions;
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
        Context.SetUInt8Node(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldInt8Panel_FinishedEditing(object? sender, Int8EditedEventArgs e)
    {
        Context.SetInt8Node(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldUInt16Panel_FinishedEditing(object? sender, UInt16EditedEventArgs e)
    {
        Context.SetUInt16Node(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldInt16Panel_FinishedEditing(object? sender, Int16EditedEventArgs e)
    {
        Context.SetInt16Node(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldUInt32Panel_FinishedEditing(object? sender, UInt32EditedEventArgs e)
    {
        Context.SetUInt32Node(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldInt32Panel_FinishedEditing(object? sender, Int32EditedEventArgs e)
    {
        Context.SetInt32Node(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldUInt64Panel_FinishedEditing(object? sender, UInt64EditedEventArgs e)
    {
        Context.SetUInt64Node(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldInt64Panel_FinishedEditing(object? sender, Int64EditedEventArgs e)
    {
        Context.SetInt64Node(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldSinglePanel_FinishedEditing(object? sender, SingleEditedEventArgs e)
    {
        Context.SetSingleNode(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldDoublePanel_FinishedEditing(object? sender, DoubleEditedEventArgs e)
    {
        Context.SetDoubleNode(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldStringPanel_FinishedEditing(object? sender, StringEditedEventArgs e)
    {
        Context.SetStringNode(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldResRefPanel_FinishedEditing(object? sender, ResRefEditedEventArgs e)
    {
        Context.SetResRefNode(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldLocalizedStringPanel_FinishedEditing(object? sender, LocalizedStringEditedEventArgs e)
    {
        Context.SetLocalizedStringNode(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldBinaryPanel_FinishedEditing(object? sender, BinaryEditedEventArgs e)
    {
        Context.SetBinaryNode(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldVector3Panel_FinishedEditing(object? sender, Vector3EditedEventArgs e)
    {
        Context.SetVector3Node(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldVector4Panel_FinishedEditing(object? sender, Vector4EditedEventArgs e)
    {
        Context.SetVector4Node(Context.SelectedNode.Path, e.NewValue);
    }
    private void FieldStructPanel_FinishedEditing(object? sender, StructEditedEventArgs e)
    {
        Context.SetStructNode(Context.SelectedNode.Path, e.NewValue);
    }
    private void LabelInput_LostFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is TextBox textbox && textbox.Text != Context.SelectedNode.Label)
        {
            var path = Context.RootNode.GetPathOf(Context.SelectedNode);
            Context.Relabel(path, textbox.Text ?? "");
        }
    }

    private async Task<ContextMenu> GetStructContextMenu(BaseGFFNodeViewModel node)
    {
        var menu = new ContextMenu();

        if (node is IStructGFFNodeViewModel dataAsStruct)
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
        else if (node is ListGFFNodeViewModel dataAsList)
        {
            menu.Items.Add(new MenuItem() { Header = "Add Struct", Command = ReactiveCommand.Create(() => AddStruct(dataAsList)) });
        }

        if (menu.Items.Count > 0)
        {
            menu.Items.Add(new Separator());
        }

        if (node is ListGFFNodeViewModel)
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

    private void AddUInt8(IStructGFFNodeViewModel parent)
    {
        var label = "New UInt8".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetUInt8Node(path, default);
    }
    private void AddInt8(IStructGFFNodeViewModel parent)
    {
        var label = "New Int8".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetInt8Node(path, default);
    }
    private void AddUInt16(IStructGFFNodeViewModel parent)
    {
        var label = "New UInt16".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetUInt16Node(path, default);
    }
    private void AddInt16(IStructGFFNodeViewModel parent)
    {
        var label = "New Int16".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetInt16Node(path, default);
    }
    private void AddUInt32(IStructGFFNodeViewModel parent)
    {
        var label = "New UInt32".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetUInt32Node(path, default);
    }
    private void AddInt32(IStructGFFNodeViewModel parent)
    {
        var label = "New Int32".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetInt32Node(path, default);
    }
    private void AddUInt64(IStructGFFNodeViewModel parent)
    {
        var label = "New UInt64".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetUInt64Node(path, default);
    }
    private void AddInt64(IStructGFFNodeViewModel parent)
    {
        var label = "New Int64".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetUInt64Node(path, default);
    }
    private void AddSingle(IStructGFFNodeViewModel parent)
    {
        var label = "New Single".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetSingleNode(path, default);
    }
    private void AddDouble(IStructGFFNodeViewModel parent)
    {
        var label = "New Double".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetDoubleNode(path, default);
    }
    private void AddResRef(IStructGFFNodeViewModel parent)
    {
        var label = "New ResRef".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetResRefNode(path, new());
    }
    private void AddString(IStructGFFNodeViewModel parent)
    {
        var label = "New String".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetStringNode(path, "");
    }
    private void AddLocalizedString(IStructGFFNodeViewModel parent)
    {
        var label = "New Localized String".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetLocalizedStringNode(path, new());
    }
    private void AddBinary(IStructGFFNodeViewModel parent)
    {
        var label = "New Binary".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetBinaryNode(path, []);
    }
    private void AddVector3(IStructGFFNodeViewModel parent)
    {
        var label = "New Vector3".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetVector3Node(path, new());
    }
    private void AddVector4(IStructGFFNodeViewModel parent)
    {
        var label = "New Vector4".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetVector4Node(path, new());
    }
    private void AddStruct(IStructGFFNodeViewModel parent)
    {
        var label = "New Struct".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetStructNode(path, default);
    }
    private void AddStruct(ListGFFNodeViewModel parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend(parent.Children.Count);
        Context.SetStructNode(path, default);
    }
    private void AddList(IStructGFFNodeViewModel parent)
    {
        var label = "New List".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
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

        if (await HasStructOnClipboard() && selectedNode is ListGFFNodeViewModel selectedListNode)
        {
            var text = await Clipboard!.GetTextAsync() ?? "";
            var node = (ListStructGFFNodeViewModel)deserializeNodeService.Deserialize(selectedNode, text);
            selectedListNode.AddStruct(node);
        }
        else if (await HasFieldOnClipboard() && selectedNode is IStructGFFNodeViewModel selectedStructNode)
        {
            var text = await Clipboard!.GetTextAsync() ?? "";
            var node = (BaseFieldGFFNodeViewModel)deserializeNodeService.Deserialize(selectedNode, text);
            node.Label = node.Label.GetUniqueLabel(selectedStructNode);
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
            var clipboard = await Clipboard!.GetTextAsync() ?? "";
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
            var clipboard = await Clipboard!.GetTextAsync() ?? "";
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
