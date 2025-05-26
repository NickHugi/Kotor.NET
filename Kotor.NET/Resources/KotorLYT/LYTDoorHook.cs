using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorLYT;

public class LYTDoorHook
{
    public ResRef Room { get; set; }
    public string Door { get; set; }
    public int Unknown { get; set; }
    public Vector3 Position { get; set; }
    public Vector4 Orientation { get; set; }
    public int Index => _lyt._doorHooks.IndexOf(this);

    private LYT _lyt;

    internal LYTDoorHook(LYT lyt, ResRef room, string door, int unknown, Vector3 position, Vector4 orientation)
    {
        _lyt = lyt;
        Room = room;
        Door = door;
        Unknown = unknown;
        Position = position;
        Orientation = orientation;
    }

    public void Remove()
    {
        _lyt._doorHooks.Remove(this);
    }
}
