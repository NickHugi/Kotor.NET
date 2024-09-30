using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.AsciiLYT;

public class LYTAscii
{
    public string FileDependency { get; set; } = "";
    public LYTAsciiLayout Layout { get; set; } = new();

    public LYTAscii()
    {
    }
    public LYTAscii(Stream stream)
    {
        var reader = new StreamReader(stream);

        while (true)
        {
            var line = reader.ReadLine();

            if (line is null)
                break;

            var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var command = tokens.FirstOrDefault()?.ToLower();

            if (command is null)
                continue;

            if (command == "filedependancy")
                FileDependency = tokens.ElementAt(1);
            if (command == "beginlayout")
                Layout = new LYTAsciiLayout(reader);
        }
    }

    public void Write(Stream stream)
    {
        var writer = new StreamWriter(stream);

        if (string.IsNullOrWhiteSpace(FileDependency) == false)
            writer.WriteLine($"filedependancy {FileDependency}");

        Layout.Write(writer);
    }
}
