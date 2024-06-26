using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryTGA;

public enum TGABinaryImageType
{
    NoImage = 0,

    ColourMapped = 1,
    RGB = 2,
    BlackAndWhite = 3,

    RLE_ColourMapped = 9,
    RLE_RGB = 10,
    RLE_BlackAndWhite = 11,
}
