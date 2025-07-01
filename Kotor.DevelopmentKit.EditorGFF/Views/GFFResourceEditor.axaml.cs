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
using Kotor.DevelopmentKit.Base;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.Settings;
using Kotor.DevelopmentKit.Base.Settings.ViewModels;
using Kotor.DevelopmentKit.Base.Views;
using Kotor.DevelopmentKit.EditorGFF.Actions;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.Extensions;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.Services;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.Kotor2DA;
using Kotor.NET.Resources.KotorGFF;
using Microsoft.Extensions.DependencyInjection;
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

    public void AddUInt8(IStructGFFNode parent)
    {
        var label = "New UInt8".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetUInt8Node(path, default);
    }

    public void AddInt8(IStructGFFNode parent)
    {
        var label = "New Int8".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetInt8Node(path, default);
    }

    public void AddUInt16(IStructGFFNode parent)
    {
        var label = "New UInt16".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetUInt16Node(path, default);
    }

    public void AddInt16(IStructGFFNode parent)
    {
        var label = "New Int16".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetInt16Node(path, default);
    }

    public void AddUInt32(IStructGFFNode parent)
    {
        var label = "New UInt32".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetUInt32Node(path, default);
    }

    public void AddInt32(IStructGFFNode parent)
    {
        var label = "New Int32".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetInt32Node(path, default);
    }

    public void AddUInt64(IStructGFFNode parent)
    {
        var label = "New UInt64".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetUInt64Node(path, default);
    }

    public void AddInt64(IStructGFFNode parent)
    {
        var label = "New Int64".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetUInt64Node(path, default);
    }

    public void AddSingle(IStructGFFNode parent)
    {
        var label = "New Single".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetSingleNode(path, default);
    }

    public void AddDouble(IStructGFFNode parent)
    {
        var label = "New Double".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetDoubleNode(path, default);
    }

    public void AddResRef(IStructGFFNode parent)
    {
        var label = "New ResRef".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetResRefNode(path, new());
    }

    public void AddString(IStructGFFNode parent)
    {
        var label = "New String".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetStringNode(path, "");
    }

    public void AddLocalizedString(IStructGFFNode parent)
    {
        var label = "New Localized String".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetLocalizedStringNode(path, new());
    }

    public void AddBinary(IStructGFFNode parent)
    {
        var label = "New Binary".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetBinaryNode(path, []);
    }

    public void AddVector3(IStructGFFNode parent)
    {
        var label = "New Vector3".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetVector3Node(path, new());
    }

    public void AddVector4(IStructGFFNode parent)
    {
        var label = "New Vector4".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetVector4Node(path, new());
    }

    public void AddStruct(IStructGFFNode parent)
    {
        var label = "New Struct".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetStructNode(path, default);
    }

    public void AddStruct(ListGFFNode parent)
    {
        var path = Context.RootNode.GetPathOf(parent).Extend(parent.Children.Count);
        Context.SetStructNode(path, default);
    }

    public void AddList(IStructGFFNode parent)
    {
        var label = "New List".GetUniqueLabel(parent);
        var path = Context.RootNode.GetPathOf(parent).Extend(label);
        Context.SetList(path);
    }

    public async Task CutNode(BaseGFFNode node)
    {
        await CopyNode(node);
        DeleteNode(node);
    }

    public void DeleteNode(BaseGFFNode node)
    {
        Context.DeleteNode(node);
    }

    public async Task CopyNode(BaseGFFNode node)
    {
        var serilizeService = new NodeSerializer();
        var text = serilizeService.Serialize(node);
        await Clipboard!.SetTextAsync(text);
    }

    public async Task PasteNode()
    {
        var deserializeNodeService = new NodeDeserializer();
        var selectedNode = Context.SelectedNode;

        if (await HasStructOnClipboard() && selectedNode is ListGFFNode selectedListNode)
        {
            var text = await Clipboard!.GetTextAsync() ?? "";
            var node = (ListStructGFFNode)deserializeNodeService.Deserialize(selectedNode, text);
            selectedListNode.AddStruct(node);
        }
        else if (await HasFieldOnClipboard() && selectedNode is IStructGFFNode selectedStructNode)
        {
            var text = await Clipboard!.GetTextAsync() ?? "";
            var node = (BaseFieldGFFNode)deserializeNodeService.Deserialize(selectedNode, text);
            node.Label = node.Label.GetUniqueLabel(selectedStructNode);
            selectedStructNode.AddField(node);
        }
        else
        {
            throw new NotSupportedException();
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

    public async Task OpenSettings()
    {
        var viewModel = App.ServiceProvider.GetService<SettingsDialogViewModel>();
        await new SettingsDialog()
        {
            DataContext = viewModel
        }.ShowDialog(this);
    }

    public async Task<bool> HasStructOnClipboard()
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

    public async Task<bool> HasFieldOnClipboard()
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

    private async Task<ContextMenu> GetStructContextMenu(BaseGFFNode node)
    {
        var menu = new ContextMenu();

        if (node is IStructGFFNode dataAsStruct)
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
        else if (node is ListGFFNode dataAsList)
        {
            menu.Items.Add(new MenuItem() { Header = "Add Struct", Command = ReactiveCommand.Create(() => AddStruct(dataAsList)) });
        }

        if (menu.Items.Count > 0)
        {
            menu.Items.Add(new Separator());
        }

        if (node is ListGFFNode)
        {
            menu.Items.Add(new MenuItem() { Header = "Paste Struct", Command = ReactiveCommand.Create(() => PasteNode()), IsEnabled = await HasStructOnClipboard() });
            menu.Items.Add(new MenuItem() { Header = "Copy Field", Command = ReactiveCommand.Create(async () => await CopyNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Cut Field", Command = ReactiveCommand.Create(async () => await CutNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Delete Field", Command = ReactiveCommand.Create(() => DeleteNode(node)) });
        }
        else if (node is RootStructGFFNode)
        {
            menu.Items.Add(new MenuItem() { Header = "Paste Field", Command = ReactiveCommand.Create(() => PasteNode()), IsEnabled = await HasFieldOnClipboard() });
        }
        else if (node is ListStructGFFNode)
        {
            menu.Items.Add(new MenuItem() { Header = "Paste Field", Command = ReactiveCommand.Create(() => PasteNode()), IsEnabled = await HasFieldOnClipboard() });
            menu.Items.Add(new MenuItem() { Header = "Copy Struct", Command = ReactiveCommand.Create(async () => await CopyNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Cut Struct", Command = ReactiveCommand.Create(async () => await CutNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Delete Struct", Command = ReactiveCommand.Create(() => DeleteNode(node)) });
        }
        else if (node is FieldStructGFFNode)
        {
            menu.Items.Add(new MenuItem() { Header = "Paste Field", Command = ReactiveCommand.Create(() => PasteNode()), IsEnabled = await HasFieldOnClipboard() });
            menu.Items.Add(new MenuItem() { Header = "Copy Field", Command = ReactiveCommand.Create(async () => await CopyNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Cut Field", Command = ReactiveCommand.Create(async () => await CutNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Delete Field", Command = ReactiveCommand.Create(() => DeleteNode(node)) });
        }
        else if (node is BaseFieldGFFNode)
        {
            menu.Items.Add(new MenuItem() { Header = "Copy Field", Command = ReactiveCommand.Create(async () => await CopyNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Cut Field", Command = ReactiveCommand.Create(async () => await CutNode(node)) });
            menu.Items.Add(new MenuItem() { Header = "Delete Field", Command = ReactiveCommand.Create(() => DeleteNode(node)) });
        }

        return menu;
    }

    private async void TreeDataGrid_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed)
        {
            var position = e.GetPosition(TreeDataGrid);
            var visual = (Visual?)TreeDataGrid.InputHitTest(position);
            var row = visual?.FindAncestorOfType<TreeDataGridRow>();
            var data = (BaseGFFNode)TreeDataGrid?.RowSelection?.SelectedItem!;

            if (row != null)
            {
                var menu = await GetStructContextMenu(data);
                menu.Open(row);
            }
        }
    }

    private void FieldUInt8Panel_FinishedEditing(object? sender, UInt8EditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetUInt8Node(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldInt8Panel_FinishedEditing(object? sender, Int8EditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetInt8Node(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldUInt16Panel_FinishedEditing(object? sender, UInt16EditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetUInt16Node(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldInt16Panel_FinishedEditing(object? sender, Int16EditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetInt16Node(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldUInt32Panel_FinishedEditing(object? sender, UInt32EditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetUInt32Node(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldInt32Panel_FinishedEditing(object? sender, Int32EditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetInt32Node(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldUInt64Panel_FinishedEditing(object? sender, UInt64EditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetUInt64Node(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldInt64Panel_FinishedEditing(object? sender, Int64EditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetInt64Node(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldSinglePanel_FinishedEditing(object? sender, SingleEditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetSingleNode(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldDoublePanel_FinishedEditing(object? sender, DoubleEditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetDoubleNode(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldStringPanel_FinishedEditing(object? sender, StringEditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetStringNode(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldResRefPanel_FinishedEditing(object? sender, ResRefEditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetResRefNode(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldLocalizedStringPanel_FinishedEditing(object? sender, LocalizedStringEditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetLocalizedStringNode(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldBinaryPanel_FinishedEditing(object? sender, BinaryEditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetBinaryNode(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldVector3Panel_FinishedEditing(object? sender, Vector3EditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetVector3Node(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldVector4Panel_FinishedEditing(object? sender, Vector4EditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetVector4Node(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void FieldStructPanel_FinishedEditing(object? sender, StructEditedEventArgs e)
    {
        if (Context.SelectedNode is not null)
        {
            Context.SetStructNode(Context.SelectedNode.Path, e.NewValue);
        }
    }

    private void LabelInput_LostFocus(object? sender, RoutedEventArgs e)
    {
        if (Context.SelectedNode is null)
            return;
        if (sender is not TextBox textbox)
            return;
        if (textbox.Text == Context.SelectedNode.Label)
            return;

        var path = Context.RootNode.GetPathOf(Context.SelectedNode);
        Context.Relabel(path, textbox.Text ?? "");
    }
}
