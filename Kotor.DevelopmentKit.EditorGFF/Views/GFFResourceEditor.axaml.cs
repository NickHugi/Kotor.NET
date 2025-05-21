using System;
using System.Collections.Generic;
using System.Linq;
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
    public GFFResourceEditorViewModel Context => (GFFResourceEditorViewModel)DataContext!;

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
    
    private void TreeDataGrid_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed)
        {
            var position = e.GetPosition(TreeDataGrid);
            var visual = (Visual?)TreeDataGrid.InputHitTest(position);
            var row = visual?.FindAncestorOfType<TreeDataGridRow>();
            var data = (IGFFTreeNodeViewModel)TreeDataGrid?.RowSelection?.SelectedItem!;

            if (row != null)
            {
                var menu = GetStructContextMenu(data);

                menu.Open(row);
            }
        }
    }
    private void FieldUInt8Panel_FinishedEditing(object? sender, UInt8EditedEventArgs e)
    {
        var action = new SetUInt8Action(Context.GetPathOf(e.ViewModel), e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldInt8Panel_FinishedEditing(object? sender, Int8EditedEventArgs e)
    {
        var action = new SetInt8Action(e.ViewModel, e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldUInt16Panel_FinishedEditing(object? sender, UInt16EditedEventArgs e)
    {
        var action = new SetUInt16Action(Context.GetPathOf(e.ViewModel), e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldInt16Panel_FinishedEditing(object? sender, Int16EditedEventArgs e)
    {
        var action = new SetInt16Action(e.ViewModel, e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldUInt32Panel_FinishedEditing(object? sender, UInt32EditedEventArgs e)
    {
        var action = new SetUInt32Action(e.ViewModel, e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldInt32Panel_FinishedEditing(object? sender, Int32EditedEventArgs e)
    {
        var action = new SetInt32Action(e.ViewModel, e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldUInt64Panel_FinishedEditing(object? sender, UInt64EditedEventArgs e)
    {
        var action = new SetUInt64Action(e.ViewModel, e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldInt64Panel_FinishedEditing(object? sender, Int64EditedEventArgs e)
    {
        var action = new SetInt64Action(e.ViewModel, e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldSinglePanel_FinishedEditing(object? sender, SingleEditedEventArgs e)
    {
        var action = new SetSingleAction(e.ViewModel, e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldDoublePanel_FinishedEditing(object? sender, DoubleEditedEventArgs e)
    {
        var action = new SetDoubleAction(e.ViewModel, e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldStringPanel_FinishedEditing(object? sender, StringEditedEventArgs e)
    {
        var action = new SetStringAction(e.ViewModel, e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldResRefPanel_FinishedEditing(object? sender, ResRefEditedEventArgs e)
    {
        var action = new SetResRefAction(e.ViewModel, e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldLocalizedStringPanel_FinishedEditing(object? sender, LocalizedStringEditedEventArgs e)
    {
        var action = new SetLocalizedStringAction(e.ViewModel, e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldBinaryPanel_FinishedEditing(object? sender, BinaryEditedEventArgs e)
    {
        var action = new SetBinaryAction(e.ViewModel, e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldVector3Panel_FinishedEditing(object? sender, Vector3EditedEventArgs e)
    {
        var action = new SetVector3Action(e.ViewModel, e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldVector4Panel_FinishedEditing(object? sender, Vector4EditedEventArgs e)
    {
        var action = new SetVector4Action(e.ViewModel, e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }
    private void FieldStructPanel_FinishedEditing(object? sender, StructEditedEventArgs e)
    {
        var action = new SetStructAction(e.ViewModel, e.OldValue, e.NewValue);
        Context.History.Apply(action);
    }

    private ContextMenu GetStructContextMenu(IGFFTreeNodeViewModel data)
    {
        var menu = new ContextMenu();

        if (data is BaseStructGFFTreeNodeViewModel dataAsStruct)
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
        else if (data is ListGFFTreeNodeViewModel dataAsList)
        {
            menu.Items.Add(new MenuItem() { Header = "Add Struct", Command = ReactiveCommand.Create(() => dataAsList.AddStruct()) });
        }

        if (menu.Items.Count > 0)
            menu.Items.Add(new Separator());

        if (data is StructInListGFFTreeNodeViewModel dataAsStructInList)
        {
            menu.Items.Add(new MenuItem() { Header = "Copy Struct", Command = ReactiveCommand.Create(() => { }) });
            menu.Items.Add(new MenuItem() { Header = "Cut Struct", Command = ReactiveCommand.Create(() => { }) });
            menu.Items.Add(new MenuItem() { Header = "Delete Struct", Command = ReactiveCommand.Create(() => data.Delete()) });
        }
        else if (data is IFieldGFFTreeNodeViewModel field)
        {
            menu.Items.Add(new MenuItem() { Header = "Copy Field", Command = ReactiveCommand.Create(() => { }) });
            menu.Items.Add(new MenuItem() { Header = "Cut Field", Command = ReactiveCommand.Create(() => { }) });
            menu.Items.Add(new MenuItem() { Header = "Delete Field", Command = ReactiveCommand.Create(() => Context.DeleteField(field)) });
        }

        return menu;
    }

    private void AddUInt8(BaseStructGFFTreeNodeViewModel parent)
    {
        object[] path = [.. Context.GetPathOf(parent), "New UInt8"];
        var action = new SetUInt8Action(path, null, 0);
        Context.History.Apply(action);
    }
    private void AddInt8(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new Int8GFFTreeNodeViewModel(parent, "New Int8"));
    }
    private void AddUInt16(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new UInt16GFFTreeNodeViewModel(parent, "New UInt16"));
    }
    private void AddInt16(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new Int16GFFTreeNodeViewModel(parent, "New Int16"));
    }
    private void AddUInt32(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new UInt32GFFTreeNodeViewModel(parent, "New UInt32"));
    }
    private void AddInt32(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new Int32GFFTreeNodeViewModel(parent, "New Int32"));
    }
    private void AddUInt64(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new UInt64GFFTreeNodeViewModel(parent, "New UInt64"));
    }
    private void AddInt64(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new Int64GFFTreeNodeViewModel(parent, "New UInt64"));
    }
    private void AddSingle(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new SingleGFFTreeNodeViewModel(parent, "New Single"));
    }
    private void AddDouble(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new DoubleGFFTreeNodeViewModel(parent, "New Double"));
    }
    private void AddResRef(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new ResRefGFFTreeNodeViewModel(parent, "New ResRef"));
    }
    private void AddString(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new StringGFFTreeNodeViewModel(parent, "New String"));
    }
    private void AddLocalizedString(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new LocalizedStringGFFTreeNodeViewModel(parent, "New Localized String"));
    }
    private void AddBinary(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new BinaryGFFTreeNodeViewModel(parent, "New Binary"));
    }
    private void AddVector3(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new Vector3GFFTreeNodeViewModel(parent, "New Vector3"));
    }
    private void AddVector4(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new Vector4GFFTreeNodeViewModel(parent, "New Vector4"));
    }
    private void AddStruct(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new StructGFFTreeNodeViewModel(parent, "New Struct"));
    }
    private void AddList(BaseStructGFFTreeNodeViewModel parent)
    {
        parent.AddField(new ListGFFTreeNodeViewModel(parent, "New List"));
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
