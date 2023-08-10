using KotorDotNET.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Extensions
{
    public static class BinaryWriterExtensions
    {
        public static void Write(this BinaryWriter writer, string value, int prefixSize)
        {
            if (prefixSize == 0)
            {
                writer.Write(value.ToCharArray());
            }
            else if (prefixSize == 1)
            {
                writer.Write((byte)value.Length);
            }
            else if (prefixSize == 2)
            {
                writer.Write((ushort)value.Length);
            }
            else if (prefixSize == 4)
            {
                writer.Write((uint)value.Length);
            }
            else if (prefixSize == 8)
            {
                writer.Write((ulong)value.Length);
            }
            else
            {
                throw new ArgumentException("Unsupported prefix size specified.");
            }
        }

        public static void Write(this BinaryWriter writer, ResRef value)
        {
            Write(writer, value.Get().PadRight(16, '\0').Truncate(16), 1);
        }
    }
}
