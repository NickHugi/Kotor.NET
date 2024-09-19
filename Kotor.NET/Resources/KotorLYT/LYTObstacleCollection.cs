using System.Collections;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorLYT;

public class LYTObstacleCollection : IEnumerable<LYTObstacle>
{
    private LYT _lyt;

    internal LYTObstacleCollection(LYT lyt)
    {
        _lyt = lyt;
    }

    public IEnumerator<LYTObstacle> GetEnumerator() => _lyt._obstacles.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _lyt._obstacles.GetEnumerator();

    public LYTObstacle Add(ResRef room, float x, float y, float z)
    {
        var obstacle = new LYTObstacle(_lyt, room, new(x, y, z));
        _lyt._obstacles.Add(obstacle);
        return obstacle;
    }
}
