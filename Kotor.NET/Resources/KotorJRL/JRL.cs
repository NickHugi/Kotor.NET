using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorJRL;

public class JRL
{
    public GFF Source { get; }

    public JRL()
    {
        Source = new();
    }
    public JRL(GFF source)
    {
        Source = source;
    }
    public static JRL FromFile(string filepath)
    {
        return new(GFF.FromFile(filepath));
    }
    public static JRL FromBytes(byte[] bytes)
    {
        return new(GFF.FromBytes(bytes));
    }
    public static JRL FromStream(Stream stream)
    {
        return new(GFF.FromStream(stream));
    }

    public JRLCategoryCollection Categories => new(Source);
}
