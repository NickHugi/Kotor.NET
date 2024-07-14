using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorGFF;

public class GFF
{
    public GFFType Type { get; set; }
    public GFFStruct Root { get; set; } = new();
}

public enum GFFType
{
    GFF,
    ARE,
    IFO,
    GIT,
    UTI,
    UTC,
    DLG,
    ITP,
    UTT,
    UTS,
    FAC,
    UTE,
    UTD,
    UTP,
    GUI,
    UTM,
    JRL,
    UTW,
    PTH,
}
