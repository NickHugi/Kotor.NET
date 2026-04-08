using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.AsciiVIS;

public class VISAscii
{
    public List<VISAsciiRoom> Rooms { get; set; } = new();

    public VISAscii()
    {
    }
    public VISAscii(Stream stream)
    {
        try
        {
            var reader = new StreamReader(stream);

            while (true)
            {
                var line = reader.ReadLine();

                if (line is null)
                    break;

                var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var model = tokens.ElementAt(0);
                var count = int.Parse(tokens.ElementAt(1));

                var room = new VISAsciiRoom(reader, model, count);
                Rooms.Add(room);
            }
        }
        catch (Exception ex)
        {
            throw new IOException("Failed to read the 2DA data.", ex);
        }
    }

    public void Write(Stream stream)
    {
        try
        {
            var writer = new StreamWriter(stream);
            Rooms.ForEach(room => room.Write(writer));
        }
        catch (Exception ex)
        {
            throw new IOException("Failed to write the 2DA data.", ex);
        }
    }
}
