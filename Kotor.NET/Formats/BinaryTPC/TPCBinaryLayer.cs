using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryTPC;

public class TPCBinaryLayer
{
    public List<byte[]> Mipmaps { get; set; } = new();
}
