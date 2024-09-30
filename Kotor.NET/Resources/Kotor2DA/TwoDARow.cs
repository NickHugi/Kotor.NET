namespace Kotor.NET.Resources.Kotor2DA;

public class TwoDARow
{
    public string RowHeader { get; set; }
    public int Index => _twoda._rows.IndexOf(this);

    internal readonly Dictionary<string, string> _cells;
    internal readonly TwoDA _twoda;

    internal TwoDARow(TwoDA twoda)
    {
        _twoda = twoda;
        _cells = new Dictionary<string, string>();
        RowHeader = "";
    }

    public TwoDACell GetCell(string underColumn)
    {
        return new(_twoda, this, underColumn);
    }
}
