using Kotor.NET.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Extensions;

public static class BinaryReaderExtensions
{
    /// <summary>
    /// Reads 16 bytes from the stream and returns a ResRef.
    /// </summary>
    /// <param name="reader">The BinaryReader containing the stream.</param>
    /// <returns>The ResRef read from the stream.</returns>
    public static ResRef ReadResRef(this BinaryReader reader)
    {
        var text = reader.ReadString(16).Replace("\0", "").Split('\0').First();
        var resref = new ResRef(text);
        return resref;
    }

    /// <summary>
    /// Reads a string of a given length from the stream.
    /// </summary>
    /// <param name="reader">The BinaryReader containing the streem.</param>
    /// <param name="length">The number of bytes to read.</param>
    /// <param name="nullTerminated">Trim the string at the first null character.</param>
    /// <param name="encoding">The encoding used to decode the byte stream.</param>
    /// <returns>The string read from the stream.</returns>
    public static string ReadString(this BinaryReader reader, int length, bool nullTerminated = true, Encoding? encoding = null)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var data = reader.ReadBytes(length);
        if (encoding is null)
        {
            encoding = Encoding.GetEncoding(1252);
        }
        var text = Encoding.GetEncoding(1252).GetString(data);
        if (nullTerminated)
        {
            text = text.Split('\0').First();
        }
        return text;
    }

    /// <summary>
    /// Continously reads from the stream until a given character terminator
    /// has been reached.
    /// </summary>
    /// <param name="reader">The BinaryReader containing the stream.</param>
    /// <param name="terminator">The character to stop reading at.</param>
    /// <returns>The string read from the stream.</returns>
    public static string ReadTerminatedString(this BinaryReader reader, char terminator)
    {
        var builder = new StringBuilder();

        while (reader.PeekChar() != terminator)
        {
            builder.Append(reader.ReadChar());
        }

        reader.BaseStream.Position += 1;

        return builder.ToString();
    }

    public static Vector2 ReadVector2(this BinaryReader reader)
    {
        return new Vector2()
        {
            X = reader.ReadSingle(),
            Y = reader.ReadSingle()
        };
    }

    public static Vector3 ReadVector3(this BinaryReader reader)
    {
        return new Vector3()
        {
            X = reader.ReadSingle(),
            Y = reader.ReadSingle(),
            Z = reader.ReadSingle()
        };
    }

    public static Vector4 ReadVector4(this BinaryReader reader, bool startWithWComponent = false)
    {
        if (startWithWComponent)
        {
            return new Vector4()
            {
                W = reader.ReadSingle(),
                Y = reader.ReadSingle(),
                Z = reader.ReadSingle(),
                X = reader.ReadSingle()
            };
        }
        else
        {
            return new Vector4()
            {
                X = reader.ReadSingle(),
                Y = reader.ReadSingle(),
                Z = reader.ReadSingle(),
                W = reader.ReadSingle()
            };
        }
    }

    public static BoundingBox ReadBoundingBox(this BinaryReader reader)
    {
        var min = reader.ReadVector3();
        var max = reader.ReadVector3();
        return new BoundingBox(min, max);
    }
}
