using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.AsciiLYT;

public class LYTAsciiDoorHook
{
    public string Room
    {
        get => _tokens[0];
        set => _tokens[0] = value;
    }
    public string Door
    {
        get => _tokens[1];
        set => _tokens[1] = value;
    }
    public string Unknown
    {
        get => _tokens[2];
        set => _tokens[2] = value;
    }
    public string PositionX
    {
        get => _tokens[3];
        set => _tokens[3] = value;
    }
    public string PositionY
    {
        get => _tokens[4];
        set => _tokens[4] = value;
    }
    public string PositionZ
    {
        get => _tokens[5];
        set => _tokens[5] = value;
    }
    public string OrientationX
    {
        get => _tokens[6];
        set => _tokens[6] = value;
    }
    public string OrientationY
    {
        get => _tokens[7];
        set => _tokens[7] = value;
    }
    public string OrientationZ
    {
        get => _tokens[8];
        set => _tokens[8] = value;
    }
    public string OrientationW
    {
        get => _tokens[9];
        set => _tokens[9] = value;
    }

    private string[] _tokens;

    public LYTAsciiDoorHook()
    {
        _tokens = [ "", "", "0", "0", "0", "0", "0", "0", "0" ];
    }
    public LYTAsciiDoorHook(StreamReader reader)
    {
        var line = reader.ReadLine();
        _tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(10).ToArray();
    }

    public void Write(StreamWriter writer)
    {
        var line = string.Join("  ", _tokens);
        writer.WriteLine("      " + line);
    }
}
