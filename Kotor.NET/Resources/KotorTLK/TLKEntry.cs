using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorTLK;

public class TLKEntry
{
    public string Text { get; set; }
    public ResRef Sound { get; set; }
    public bool Exists => _tlk._entries.Contains(this);
    public int StringRef => _tlk._entries.IndexOf(this);

    private TLK _tlk;

    internal TLKEntry(TLK tlk, string text, ResRef resref)
    {
        _tlk = tlk;

        Text = text;
        Sound = resref;
    }
    internal TLKEntry(TLK tlk, string text)
    {
        _tlk = tlk;

        Text = text;
        Sound = "";
    }

    public void Remove()
    {
        _tlk._entries.Remove(this);
    }
}
