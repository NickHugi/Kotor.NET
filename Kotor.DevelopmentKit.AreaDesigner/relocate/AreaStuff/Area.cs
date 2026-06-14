using System.Collections.Generic;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Area
{
    private List<Room> _rooms = new();
    public IReadOnlyList<Room> Rooms => _rooms.AsReadOnly();

    public void AddRoom(Room room)
    {
        _rooms.Add(room);
    }
    public void DeleteRoom(Room room)
    {
        _rooms.Remove(room);
    }
}
