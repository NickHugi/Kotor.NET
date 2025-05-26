namespace Kotor.NET.Resources.KotorLIP;

public class LIPKeyFrame
{
    public float Time { get; set; }
    public LIPMouthShape Shape { get; set; }
    public int Index => _lip._frames.IndexOf(this);

    private LIP _lip;

    internal LIPKeyFrame(LIP lip, float time, LIPMouthShape shape)
    {
        _lip = lip;
        Time = time;
        Shape = shape;
    }

    public void Remove()
    {
        _lip._frames.Remove(this);
    }
}
