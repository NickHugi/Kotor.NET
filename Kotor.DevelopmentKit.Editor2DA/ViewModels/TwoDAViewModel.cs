using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using DynamicData;
using Kotor.DevelopmentKit.Base;
using Kotor.NET.Resources.Kotor2DA;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Editor2DA.ViewModels;

public class TwoDAViewModel : ReactiveObject
{

    private string _filter = "";
    public string Filter
    {
        get => _filter;
        set => this.RaiseAndSetIfChanged(ref _filter, value);
    }

    public ObservableCollection<ColumnViewModel> Columns { get; } = [];

    private SourceList<RowViewModel> _rowsSource;
    private readonly ReadOnlyObservableCollection<RowViewModel> _rows;
    public ReadOnlyObservableCollection<RowViewModel> Rows => _rows;

    public TwoDAViewModel()
    {
        _rowsSource = new();
        var sorter = new SourceListIndexComperer<RowViewModel>(_rowsSource);
        _rowsSource.Connect()
            .ObserveOn(AvaloniaScheduler.Instance)
            .AutoRefreshOnObservable(x => this.ObservableForProperty(x => x.Filter))
            .Filter(row => row.Cells.Any(cell => cell.Value.Contains(Filter, StringComparison.InvariantCultureIgnoreCase)) || row.RowHeader.Contains(Filter, StringComparison.InvariantCultureIgnoreCase))
            .Sort(sorter)
            .Bind(out _rows)
            .Subscribe();

        Load(new());
    }


    public void Load(TwoDA twoda)
    {
        var columnHeaders = twoda.GetColumns();
        var rows = twoda.GetRows().Select(row =>
        {
            return new RowViewModel()
            {
                RowHeader = row.RowHeader,
                Cells = columnHeaders.ToDictionary(x => x, x => row.GetCell(x).AsString())
            };
        });


        Columns.Clear();
        Columns.AddRange(["Row Header", .. columnHeaders]);

        _rowsSource.Clear();
        _rowsSource.AddRange(rows);
    }

    public TwoDA Build()
    {
        TwoDA twoda = new();
        var columns = Columns.Skip(1).ToList();

        columns.ToList().ForEach(columnHeader => twoda.AddColumn(columnHeader));

        _rowsSource.Items.ToList().ForEach(rows =>
        {
            var twodaRow = twoda.AddRow(rows.RowHeader);

            for (int i = 0; i < rows.Cells.Count; i++)
            {
                var columnHeader = columns.ElementAt(i);
                var value = rows.Cells[columnHeader];
                twodaRow.GetCell(columnHeader).SetString(value);
            }
        });

        return twoda;
    }

    public int GetRowID(int rowIndex)
    {
        var row = Rows.ElementAt(rowIndex);
        var rowID = _rowsSource.Items.IndexOf(row);
        return rowID;
    }

    public int GetRowIndex(int rowID)
    {
        var row = _rowsSource.Items.ElementAt(rowID);
        var rowIndex = Rows.IndexOf(row);
        return rowIndex;
    }

    public void SetRowHeader(int rowID, string value)
    {
        _rowsSource.Edit(rows =>
        {
            var row = rows[rowID];
            row.RowHeader = value;
        });
    }

    public void SetRowCell(int rowID, string columnHeader, string value)
    {
        var columnIndex = GetColumnIndex(columnHeader);

        _rowsSource.Edit(rows =>
        {
            var row = rows[rowID];
            row.Cells[columnHeader] = value;
            rows.Replace(rows[rowID], row);
        });
    }

    public string GetRowHeader(int rowID)
    {
        var row = _rowsSource.Items.ElementAt(rowID);
        return row.RowHeader;
    }

    public string GetRowCell(int rowID, string columnName)
    {
        return _rowsSource.Items[rowID].Cells[columnName];
    }

    public void AddRow()
    {
        _rowsSource.Edit(rows =>
        {
            var rowHeader = rows.Count.ToString();
            rows.Add(new()
            {
                RowHeader = rowHeader,
                Cells = Columns.Skip(1).ToDictionary(x => x.Header, x => "")
            });
        });
    }
    public void AddRow(int rowID)
    {
        _rowsSource.Edit(rows =>
        {
            var rowHeader = rows.Count.ToString();
            rows.Insert(rowID, new()
            {
                RowHeader = rowHeader,
                Cells = Columns.Skip(1).ToDictionary(x => x.Header, x => "")
            });
        });
    }

    public void RemoveRow()
    {
        _rowsSource.RemoveAt(_rowsSource.Count - 1);
    }
    public void RemoveRow(int rowID)
    {
        _rowsSource.RemoveAt(rowID);
    }

    public void AddColumn(string columnHeader)
    {
        AddColumn(columnHeader, _rowsSource.Items.Select(x => ""));
    }
    public void AddColumn(string columnHeader, IEnumerable<string> newCellValues)
    {
        _rowsSource.Edit(rows =>
        {
            for (int i = 0; i < rows.Count; i++)
            {
                rows[i].Cells.Add(columnHeader, newCellValues.ElementAtOrDefault(i) ?? "");
            }
        });

        Columns.Add(columnHeader);
    }
    public void AddColumn(string columnHeader, IEnumerable<string> newCellValues, int columnIndex)
    {
        _rowsSource.Edit(rows =>
        {
            for (int i = 0; i < rows.Count; i++)
            {
                rows[i].Cells.Add(columnHeader, newCellValues.ElementAtOrDefault(i) ?? "");
            }
        });

        Columns.Insert(columnIndex, columnHeader);
    }

    public void RemoveColumn(string columnHeader)
    {
        var columnIndex = GetColumnIndex(columnHeader);

        _rowsSource.Edit(rows =>
        {
            foreach (var row in rows)
            {
                row.Cells.Remove(columnHeader);
            }
        });

        Columns.RemoveAt(columnIndex);
    }

    public int GetColumnIndex(string columnHeader)
    {
        return Columns.ToList().FindIndex(x => x.Header == columnHeader);
    }

    public void ResetRowHeaders()
    {
        _rowsSource.Edit(rows =>
        {
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                row.RowHeader = i.ToString();
            }
        });
    }
}
