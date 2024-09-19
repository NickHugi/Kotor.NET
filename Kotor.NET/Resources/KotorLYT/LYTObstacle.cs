using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorLYT;

public class LYTObstacle
{
    public ResRef Room { get; set; }
    public Vector3 Position { get; set; }
    public int Index => _lyt._obstacles.IndexOf(this);

    private LYT _lyt;

    internal LYTObstacle(LYT lyt, ResRef room, Vector3 position)
    {
        _lyt = lyt;
        Room = room;
        Position = position;
    }

    public void Remove()
    {
        _lyt._obstacles.Remove(this);
    }
}
