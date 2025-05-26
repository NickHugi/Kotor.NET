using System.Collections;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorLYT;

public class LYTTrackCollection : IEnumerable<LYTTrack>
{
    private LYT _lyt;

    internal LYTTrackCollection(LYT lyt)
    {
        _lyt = lyt;
    }

    public IEnumerator<LYTTrack> GetEnumerator() => _lyt._tracks.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _lyt._tracks.GetEnumerator();

    public LYTTrack Add(ResRef room, float x, float y, float z)
    {
        var track = new LYTTrack(_lyt, room, new(x, y, z));
        _lyt._tracks.Add(track);
        return track;
    }
}
