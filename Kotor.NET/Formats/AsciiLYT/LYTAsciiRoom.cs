using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.AsciiLYT;

public class LYTAsciiRoom
{
    public string Model
    {
        get => _tokens[0];
        set => _tokens[0] = value;
    }
    public string PositionX
    {
        get => _tokens[1];
        set => _tokens[1] = value;
    }
    public string PositionY
    {
        get => _tokens[2];
        set => _tokens[2] = value;
    }
    public string PositionZ
    {
        get => _tokens[3];
        set => _tokens[3] = value;
    }

    private string[] _tokens;

    public LYTAsciiRoom()
    {
        _tokens = [ "", "0", "0", "0" ];
    }
    public LYTAsciiRoom(StreamReader reader)
    {
        var line = reader.ReadLine();
        _tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(4).ToArray();
    }

    public void Write(StreamWriter writer)
    {
        var line = string.Join("  ", _tokens);
        writer.WriteLine("      " + line);
    }
}
