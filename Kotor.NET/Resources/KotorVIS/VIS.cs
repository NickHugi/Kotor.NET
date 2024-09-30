using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorVIS;

public class VIS : IEnumerable<VISRoom>
{
    internal Dictionary<string, HashSet<string>> _visibility;

    public VIS()
    {
        _visibility = new();
    }

    public VISRoom Get(string room)
    {
        return new VISRoom(this, room);
    }

    public VISRoom Add(string room)
    {
        _visibility.Add(room, new());
        return new VISRoom(this, room);
    }

    public void Remove(string room)
    {
        _visibility.Remove(room);
    }

    public bool Exists(string room)
    {
        return _visibility.Keys.Contains(room);
    }

    public IEnumerator<VISRoom> GetEnumerator() => _visibility.Keys.Select(x => Get(x)).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
