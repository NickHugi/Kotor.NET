using KotorDotNET.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.FileFormats.Kotor2DA
{
    internal class TwoDABinaryStructure
    {
        public class FileRoot
        {
            public FileHeader FileHeader { get; set; }
            public List<string> Columns { get; set; } = new List<string>();
            public List<string> RowLabels { get; set; } = new List<string>();
            public List<int> CellOffsets { get; set; } = new List<int>();
            public List<string> Cells { get; set; } = new List<string>();

            public FileRoot(BinaryReader reader)
            {
                FileHeader = new FileHeader(reader);

                while (reader.PeekChar() != '\0')
                {
                    var header = reader.ReadTerminatedString('\t');
                    Columns.Add(header);
                }

                reader.BaseStream.Position += 4;

                var rowCount = reader.ReadInt32();
                var cellCount = rowCount * Columns.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    var label = reader.ReadTerminatedString('\t');
                    RowLabels.Add(label);
                }

                for (int i = 0; i < cellCount; i++)
                {
                    var cellOffset = reader.ReadUInt16();
                    CellOffsets.Add(cellOffset);
                }

                var cellDataSize = reader.ReadUInt16();
                var cellDataOffset = reader.BaseStream.Position;

                for (int i = 0; i < cellCount; i++)
                {
                    var columnIndex = i % Columns.Count;
                    var rowIndex = (int)Math.Floor(i / (decimal)Columns.Count);

                    reader.BaseStream.Position = cellDataOffset + CellOffsets[i];
                    var cellValue = reader.ReadTerminatedString('\0');
                }
            }
        }

        public class FileHeader
        {
            public string FileType { get; set; }
            public string FileVersion { get; set; }

            public FileHeader(BinaryReader reader)
            {
                FileType = reader.ReadString(4);
                FileVersion = reader.ReadString(4);
                reader.BaseStream.Position += 4;
            }
        }
    }
}
