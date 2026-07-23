using System.Collections.Generic;
using System.Linq;
using Kotor.NET.Graphics.Cameras;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Area
{
    public IReadOnlyList<Room> Rooms => _rooms.AsReadOnly();
    private List<Room> _rooms = new();

    private ICollection<Magnet> _allMagnets => Rooms.SelectMany(x => x.AllMagnets).ToList();
    public ICollection<Magnet> AllMagnets { get; private set; } = [];

    private ICollection<Magnet> _availableMagnets => AllMagnets.Where(x =>
    {
        var visible = x.Visible;

        if (x.Child is not null && x.Child.Type == WorldObjectType.Wall)
        {
            visible = visible && x.Child.Magnets.All(x => x.WorldObjectTemplate?.Type != WorldObjectType.DoorFrame);
        }

        return visible;
    }).ToList();
    public ICollection<Magnet> AvailableMagnets { get; private set; } = [];

    public void AddRoom(Room room)
    {
        _rooms.Add(room);
    }
    public void DeleteRoom(Room room)
    {
        _rooms.Remove(room);
    }

    public void Invalidate()
    {
        AllMagnets = _allMagnets;
        AvailableMagnets = _availableMagnets;
    }
}
