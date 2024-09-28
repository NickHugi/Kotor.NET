using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.AsciiTXI;

public class TXIAscii
{
    public List<TXIAsciiField> Fields { get; } = new();

    public TXIAscii()
    {
    }
    public TXIAscii(Stream stream)
    {
        var reader = new StreamReader(stream);

        while (true)
        {
            var line = reader.ReadLine();

            if (line is null)
                break;

            var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Count() == 0)
                continue;

            Fields.Add(new TXIAsciiField(reader, tokens));
        }
    }

    public void Write(Stream stream)
    {
        var writer = new StreamWriter(stream);

        Fields.ForEach(x => x.Write(writer));
    }
}
