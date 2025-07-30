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
        try
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

            if (string.IsNullOrWhiteSpace(FileDependency) == false)
                writer.WriteLine($"filedependancy {FileDependency}");

            Layout.Write(writer);
        }
        catch (Exception ex)
        {
            throw new IOException("Failed to write the 2DA data.", ex);
        }
    }
}
