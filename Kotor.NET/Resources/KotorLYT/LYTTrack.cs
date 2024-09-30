using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorLYT;

public class LYTTrack
{
    public ResRef Room { get; set; }
    public Vector3 Position { get; set; }
    public int Index => _lyt._tracks.IndexOf(this);

    private LYT _lyt;

    internal LYTTrack(LYT lyt, ResRef room, Vector3 position)
    {
        _lyt = lyt;
        Room = room;
        Position = position;
    }

    public void Remove()
    {
        _lyt._tracks.Remove(this);
    }
}
