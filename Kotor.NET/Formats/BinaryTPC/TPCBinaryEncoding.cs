using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryTPC;

public enum TPCBinaryEncoding : byte
{
    Grayscale = 1,
    RGB = 2,
    RGBA = 4,
}
