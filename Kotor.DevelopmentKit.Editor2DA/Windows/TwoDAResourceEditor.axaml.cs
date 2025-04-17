using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using DynamicData.Binding;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.Windows;
using Kotor.DevelopmentKit.Editor2DA.ViewModels;
using Kotor.DevelopmentKit.Editor2DA.Windows;
using Kotor.NET.Common.Data;
using ReactiveUI;
using DynamicData;
using Kotor.DevelopmentKit.Base;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.Base.DialogResults;
using Kotor.NET.Resources.KotorERF;
using System.IO;
using Kotor.NET.Resources.KotorRIM;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.DevelopmentKit.Editor2DA;

public partial class TwoDAResourceEditor : ResourceEditorBase<TwoDAResourceEditorViewModel, TwoDAViewModel, TwoDA>
{
    public override FilePickerFileType AllValidFilePickerFileTypes => new FilePickerFileType("All Valid Options")
    {
        Patterns = [..FilePickerTypes.TwoDA.Patterns!, ..FilePickerTypes.Encapsulated.Patterns!],
    };
    public override FilePickerOpenOptions FilePickerOpenOptions => new()
    {
        Title = "Open 2DA File",
        AllowMultiple = false,
        FileTypeFilter = [FilePickerTypes.TwoDA, FilePickerTypes.Encapsulated, AllValidFilePickerFileTypes, FilePickerTypes.All],
    };
    public override FilePickerSaveOptions FilePickerSaveOptions => new()
    {
        Title = "Save 2DA File",
        ShowOverwritePrompt = false,
        FileTypeChoices = [FilePickerTypes.TwoDA, FilePickerTypes.Encapsulated, AllValidFilePickerFileTypes, FilePickerTypes.All],
    };
    public override List<ResourceType> ResourceTypes => [ResourceType.TWODA];

    /// <summary>
    /// Track the value of the selected cell before editing has finished.
    /// </summary>
    private string _originalCellValue = "";


    public TwoDAResourceEditor()
    {
        InitializeComponent();
    }


    private void RefreshColumns()
    {
        TwodaDataGrid.Columns.Clear();

        TwodaDataGrid.Columns.Add(new DataGridTextColumn()
        {
            Header = "Row Header",
            Binding = new Binding($"RowHeader"),
            IsReadOnly = false,
        });

        for (int i = 1; i < Context.Resource.Columns.Count(); i++)
        {
            var columnHeader = Context.Resource.Columns.ElementAt(i);
            var column = new DataGridTextColumn()
            {
                Header = columnHeader,
                Binding = new Binding($"Cells[{columnHeader}]"),
                IsReadOnly = false,
            };

            TwodaDataGrid.Columns.Add(column);
        }
    }

    public async Task CopySelectedCell()
    {
        if (Clipboard is null)
            return;

        var rowIndex = TwodaDataGrid.SelectedIndex;
        var columnHeader = ((ColumnViewModel)TwodaDataGrid.CurrentColumn.Header).Header;
        var text = Context.Resource.Rows[rowIndex].Cells[columnHeader];
        await Clipboard.SetTextAsync(text);
    }

    public async Task PasteSelectedCell()
    {
        if (Clipboard is null)
            return;

        var rowIndex = Context.SelectedRowIndex;
        var rowID = Context.Resource.GetRowID(Context.SelectedRowIndex);
        var currentColumn = TwodaDataGrid.CurrentColumn;
        var columnHeader = ((ColumnViewModel)TwodaDataGrid.CurrentColumn.Header).Header;
        var newValue = await Clipboard.GetTextAsync() ?? "";

        Context.SetRowCell(rowID, columnHeader, newValue);
        TwodaDataGrid.CurrentColumn = currentColumn;
        TwodaDataGrid.SelectedIndex = rowIndex;
    }

    public void ToggleFilter()
    {
        Context.ToggleFilter();
    }

    public void ResetSorting()
    {
        TwodaDataGrid.Columns.ToList().ForEach(x => x.ClearSort());
    }

    public void ResetRowHeaders()
    {
        Context.ResetRowHeaders();
    }

    public void AddRow()
    {
        Context.AddRow();
    }

    public async void AddColumn()
    {
        var dialog = new EditColumnDialog()
        {
            Title = "Create new column",
            DataContext = new EditColumnDialogViewModel(Context.Resource.Columns.Select(x => x.Header).ToArray())
        };
        var columnHeader = await dialog.ShowDialog<string>(this);
        Context.AddColumn(columnHeader);
    }

    public void RemoveColumn(string columnHeader)
    {
        Context.RemoveColumn(columnHeader);
    }

    public void Undo()
    {
        // For whatever reason calling Undo/Redo directly from a MenuItem command causes the
        // DataGrid to display the wrong data on the affected row.
        Dispatcher.UIThread.Post(() => Context.Undo(), DispatcherPriority.Default);
    }

    public void Redo()
    {
        Dispatcher.UIThread.Post(() => Context.Redo(), DispatcherPriority.Default);
    }

    public async Task RenameColumn(string targetColumnHeader)
    {
        var dialog = new EditColumnDialog()
        {
            Title = $"Rename '{targetColumnHeader}' column",
            DataContext = new EditColumnDialogViewModel(Context.Resource.Columns.Select(x => x.Header).ToArray())
        };
        var newColumnHeader = await dialog.ShowDialog<string>(this);
        Context.RenameColumn(targetColumnHeader, newColumnHeader);
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        Context.WhenAnyValue(x => x.SelectedColumnIndex).Subscribe(x =>
        {
            var column = TwodaDataGrid.Columns.SingleOrDefault(x => x.DisplayIndex == Context.SelectedColumnIndex);
            if (column is not null && TwodaDataGrid.CurrentColumn is not null)
                TwodaDataGrid.CurrentColumn = column;
        });

        Context.Resource.WhenAnyValue(x => x.Columns.Count).Subscribe(x =>
        {
            RefreshColumns();
        });
        Context.Resource.Columns.ToObservableChangeSet().AutoRefresh(x => x.Header).Subscribe(x =>
        {
            RefreshColumns();
        });
    }


    private string GetValueFromCell(RowViewModel row, int columnIndex)
    {
        var columnHeader = Context.Resource.Columns.ElementAt(columnIndex).Header;
        return (columnIndex == 0) ? row.RowHeader : row.Cells[columnHeader];
    }

    private void SetValueToCell(DataGridTextColumn column, int rowID, string value)
    {
        if (((Binding)column.Binding).Path == "RowHeader")
        {
            Context.SetRowHeader(rowID, value, _originalCellValue);
        }
        else
        {
            Context.SetRowCell(rowID, (column.Header as ColumnViewModel)!.Header, value, _originalCellValue);
        }
    }


    private async void DataGrid_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.KeyModifiers == KeyModifiers.Control)
        {
            if (e.Key == Key.C)
            {
                await CopySelectedCell();
            }
            else if (e.Key == Key.V)
            {
                await PasteSelectedCell();
            }
        }

        Context.SelectedColumnIndex = TwodaDataGrid.CurrentColumn?.DisplayIndex ?? 0;
    }

    private void DataGrid_BeginningEdit(object? sender, Avalonia.Controls.DataGridBeginningEditEventArgs e)
    {
        var row = ((RowViewModel)e.Row.DataContext!);
        _originalCellValue = GetValueFromCell(row, e.Column.DisplayIndex);
    }

    private void DataGrid_CellEditEnded(object? sender, Avalonia.Controls.DataGridCellEditEndedEventArgs e)
    {
        var row = ((RowViewModel)e.Row.DataContext!);
        var newValue = GetValueFromCell(row, e.Column.DisplayIndex); 

        var rowID = Context.Resource.GetRowID(Context.SelectedRowIndex);

        SetValueToCell((DataGridTextColumn)e.Column, rowID, newValue);
    }

    private void DataGrid_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
    {
        Context.SelectedColumnIndex = TwodaDataGrid.CurrentColumn?.DisplayIndex ?? 0;

        if (e.InitialPressMouseButton == MouseButton.Right)
        {
            if (e.Source is Grid grid && grid.TemplatedParent is DataGridColumnHeader header)
            {
                var contextMenu = new ContextMenu();
                var headerText = (header.Content as ColumnViewModel)!.Header;
                contextMenu.Items.Add(new MenuItem { Header = "Delete Column", Command = ReactiveCommand.Create(() => RemoveColumn(headerText)), IsEnabled = (headerText != "Row Header") });
                contextMenu.Items.Add(new MenuItem { Header = "Rename Column", Command = ReactiveCommand.Create(() => RenameColumn(headerText)), IsEnabled = (headerText != "Row Header") });

                grid.ContextMenu = contextMenu;
                contextMenu.Open(grid);
            }
            else if (e.Source is Control textBlock && textBlock.TemplatedParent is DataGridCell cell)
            {

            }
        }
        e.Route = Avalonia.Interactivity.RoutingStrategies.Tunnel;
    }
}
