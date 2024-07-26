using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources;

public class TwoDA
{
    internal ISet<string> _columnHeaders { get; set; } = new HashSet<string>();
    internal List<TwoDARow> _rows { get; set; } = new();

    public TwoDARow GetRow(int index)
    {
        if (index > 0 || index <= _rows.Count())
        {
            return _rows[index];
        }
        else
        {
            throw new ArgumentException($"No row with index '{index}' exists.");
        }
    }
    public TwoDARow GetRow(string header)
    {
        var rows = GetRows(x => x.RowHeader == header);

        if (rows.Count() == 0)
        {
            throw new ArgumentException($"No row with header '{header}' exists.");
        }
        else if (rows.Count() == 1)
        {
            return rows.First();
        }
        else
        {
            throw new ArgumentException($"Multiple rows with header '{header}' exist.");
        }
    }

    public IEnumerable<TwoDARow> GetRows()
    {
        return _rows;
    }
    public IEnumerable<TwoDARow> GetRows(Func<TwoDARow, bool> predicate)
    {
        return GetRows().Where(predicate);
    }

    public TwoDARow AddRow(string header)
    {
        var row = new TwoDARow(this);
        _rows.Add(row);
        return row;
    }

    public void RemoveRow(int index)
    {
        _rows.RemoveAt(index);
    }

    public void AddColumn(string columnHeader)
    {
        if  (_columnHeaders.Contains(columnHeader))
        {
            throw new ArgumentException($"A column with the header '{columnHeader}' already exists.");
        }

        _columnHeaders.Add(columnHeader);
        _rows.ForEach(x => x._values.Add(columnHeader, ""));
    }

    public void RemoveColumn(string columnHeader)
    {
        _columnHeaders.Remove(columnHeader);
        _rows.ForEach(x => x._values.Remove(column));
    }
}

public class TwoDARow
{
    private TwoDA _twoda;
    internal Dictionary<string, string> _values;
    public string RowHeader { get; set; }

    internal TwoDARow(TwoDA twoda)
    {
        _twoda = twoda;
        _values = new();
        RowHeader = "";
    }

    public int RowIndex
    {
        get => _twoda._rows.IndexOf(this);
    }

    public TwoDACell GetCell(string columnHeader)
    {
        return new(_twoda, RowIndex, columnHeader);
    }
}

public class TwoDACell(TwoDA _twoda, int RowIndex, string _columnHeader)
{
    public void SetString(string? value)
    {
        _twoda._rows[RowIndex]._values[_columnHeader] = value ?? "";
    }
    public string? AsString()
    {
        return _twoda._rows[RowIndex]._values[_columnHeader];
    }

    public void SetInt(int? value)
    {
        _twoda._rows[RowIndex]._values[_columnHeader] = value?.ToString() ?? "";
    }
    public int? AsInt()
    {
        return int.TryParse(_twoda._rows[RowIndex]._values[_columnHeader], out var value) ? value : null;
    }

    public void SetFloat(float? value)
    {
        _twoda._rows[RowIndex]._values[_columnHeader] = value?.ToString() ?? "";
    }
    public float? AsFloat()
    {
        return float.TryParse(_twoda._rows[RowIndex]._values[_columnHeader], out var value) ? value : null;
    }

    public void SetBool(bool? value)
    {
        _twoda._rows[RowIndex]._values[_columnHeader] = (value is null) ? "" : ((value ?? false) ? "1" : "0");
    }
    public bool? AsBool()
    {
        return _twoda._rows[RowIndex]._values[_columnHeader] switch
        {
            "0" => false,
            "1" => true,
            _ => null
        };
    }
}