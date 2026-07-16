using System.Collections.Generic;
using System.Linq;
using Kotor.NET.Graphics.Cameras;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Area
{
    public IReadOnlyList<Room> Rooms => _rooms.AsReadOnly();
    private List<Room> _rooms = new();

    public ICollection<Magnet> AllMagnets
    {
        get => Rooms.SelectMany(x => x.AllMagnets).ToList();
    }

    public void AddRoom(Room room)
    {
        _rooms.Add(room);
    }
    public void DeleteRoom(Room room)
    {
        _rooms.Remove(room);
    }
}
