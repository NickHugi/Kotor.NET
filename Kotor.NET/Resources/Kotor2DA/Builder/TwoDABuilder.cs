using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.Kotor2DA.Builder;

public class TwoDABuilder
{
    private TwoDA _twoda = new();
    private bool _built = false;

    public TwoDABuilder AddColumn(string columnHeader)
    {
        _twoda.AddColumn(columnHeader);
        return this;
    }

    public TwoDABuilder AddColumns(params string[] columnHeaders)
    {
        columnHeaders.ToList().ForEach(x => AddColumn(x));
        return this;
    }

    public TwoDARowBuilder AddRow(string rowHeader)
    {
        var row = _twoda.AddRow(rowHeader);
        return new TwoDARowBuilder(this, row);
    }

    public TwoDA Build()
    {
        if (_built)
            throw new Exception(); // TODO;

        _built = true;
        return _twoda;
    }
}

public class TwoDARowBuilder
{
    private readonly TwoDABuilder _twodaBuilder;
    private readonly TwoDARow _row;

    public TwoDARowBuilder(TwoDABuilder twodaBuilder, TwoDARow row)
    {
        _twodaBuilder = twodaBuilder;
        _row = row;
    }

    public TwoDARowBuilder SetCell(string column, string value)
    {
        _row.GetCell(column).SetString(value);
        return this;
    }

    public TwoDABuilder Finish()
    {
        return _twodaBuilder;
    }
}
