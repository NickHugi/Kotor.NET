using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.AsciiVIS;

public class VISAsciiRoom
{
    public string Model { get; set; } = "";
    public List<string> Visibility { get; set; } = new();

    public VISAsciiRoom()
    {
    }
    public VISAsciiRoom(StreamReader reader, string model, int count)
    {
        Model = model;
        for (int i = 0; i < count; i++)
        {
            var line = reader.ReadLine();
            var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var otherModel = tokens.ElementAt(0);

            Visibility.Add(otherModel);
        }
    }

    public void Write(StreamWriter writer)
    {
        writer.WriteLine(Model);
        foreach (var model in Visibility)
            writer.WriteLine($"   {model}");
    }
}
