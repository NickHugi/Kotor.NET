namespace Kotor.NET.Resources.KotorVIS;

public class VISRoom
{
    private VIS _vis;
    private string _room;

    public VISRoom(VIS vis, string room)
    {
        _vis = vis;
        _room = room;
    }

    public bool CanSee(string room)
    {
        ThrowIfRoomDoesNotExist(room);
        return _vis._visibility[_room].Contains(room);
    }

    public bool CanBeSeenBy(string room)
    {
        ThrowIfRoomDoesNotExist(room);
        return _vis._visibility[room].Contains(_room);
    }

    public void SetCanSee(string room, bool visible)
    {
        ThrowIfRoomDoesNotExist(room);
        _ = visible
            ? _vis._visibility[_room].Add(room)
            : _vis._visibility[_room].Remove(room);
    }

    public void SetCanBeSeenBy(string room, bool visible)
    {
        ThrowIfRoomDoesNotExist(room);
        _ = visible
            ? _vis._visibility[room].Add(_room)
            : _vis._visibility[room].Remove(_room);
    }

    public IEnumerable<string> CanSee()
    {
        return _vis._visibility[_room].ToList();
    }

    internal void ThrowIfRoomDoesNotExist(string room)
    {
        if (!_vis.Exists(room))
            throw new ArgumentException($"The room '{room}' does not exist");
    }
}
