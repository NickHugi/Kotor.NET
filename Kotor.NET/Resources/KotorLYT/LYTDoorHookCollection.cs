using System.Collections;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorLYT;

public class LYTDoorHookCollection : IEnumerable<LYTDoorHook>
{
    private LYT _lyt;

    internal LYTDoorHookCollection(LYT lyt)
    {
        _lyt = lyt;
    }

    public IEnumerator<LYTDoorHook> GetEnumerator() => _lyt._doorHooks.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _lyt._doorHooks.GetEnumerator();

    public LYTDoorHook Add(ResRef room, string door, float positionX, float positionY, float positionZ, float orientationX, float orientationY, float orientationZ, float orientationW)
    {
        var doorHook = new LYTDoorHook(_lyt, room, door, 0, new(positionX, positionY, positionZ), new(orientationX, orientationY, orientationZ, orientationW));
        _lyt._doorHooks.Add(doorHook);
        return doorHook;
    }
}
