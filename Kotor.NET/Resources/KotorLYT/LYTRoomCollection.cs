using System.Collections;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorLYT;

public class LYTRoomCollection : IEnumerable<LYTRoom>
{
    private LYT _lyt;

    internal LYTRoomCollection(LYT lyt)
    {
        _lyt = lyt;
    }

    public IEnumerator<LYTRoom> GetEnumerator() => _lyt._rooms.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _lyt._rooms.GetEnumerator();

    public LYTRoom Add(ResRef model, float x, float y, float z)
    {
        var room = new LYTRoom(_lyt, model, new(x, y, z));
        _lyt._rooms.Add(room);
        return room;
    }
}
