using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorLYT;

public class LYTRoom
{
    public ResRef Model { get; set; }
    public Vector3 Position { get; set; }
    public int Index => _lyt._rooms.IndexOf(this);

    private LYT _lyt;

    internal LYTRoom(LYT lyt, ResRef model, Vector3 position)
    {
        _lyt = lyt;
        Model = model;
        Position = position;
    }

    public void Remove()
    {
        _lyt._rooms.Remove(this);
    }
}
