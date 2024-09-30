using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryGFF;

public enum GFFBinaryFieldType
{
    UInt8,
    Int8,
    UInt16,
    Int16,
    UInt32,
    Int32,
    UInt64,
    Int64,
    Single,
    Double,
    String,
    ResRef,
    LocalisedString,
    Binary,
    Struct,
    List,
    Vector4,
    Vector3
}
