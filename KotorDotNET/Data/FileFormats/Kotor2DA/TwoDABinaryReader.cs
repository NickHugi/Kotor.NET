using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static KotorDotNET.Data.FileFormats.Kotor2DA.TwoDABinaryStructure;

namespace KotorDotNET.Data.FileFormats.Kotor2DA
{
    public class TwoDABinaryReader : IReader<TwoDA>
    {
        private BinaryReader _binaryReader;
        private TwoDA? _twoda;

        private readonly string[] _validFileTypes = new[] { "2DA ", };
        private readonly string _validFileVersion = "V1.0";

        public TwoDABinaryReader(string filepath)
        {
            var data = System.IO.File.ReadAllBytes(filepath);
            _binaryReader = new BinaryReader(new MemoryStream(data));
        }
        public TwoDABinaryReader(byte[] data)
        {
            _binaryReader = new BinaryReader(new MemoryStream(data));
        }

        public TwoDABinaryReader(Stream stream)
        {
            _binaryReader = new BinaryReader(stream);
        }

        public TwoDA Read()
        {
            _twoda = new TwoDA();

            var file = new FileRoot(_binaryReader);

            foreach (var header in file.Columns)
            {
                _twoda.AddColumn(header);
            }

            var columnCount = file.Columns.Count;
            for (int i = 0; i < file.RowLabels.Count; i++)
            {
                var label = file.RowLabels[i];
                var row = _twoda.AddRow(label);

                for (int j = 0; j < file.Columns.Count; j++)
                {
                    var header = file.Columns[j];
                    var cellIndex = (i * file.RowLabels.Count) + j;
                    var cell = file.Cells[cellIndex];
                    row.SetCell(header, cell);
                }
            }

            return _twoda;
        }
    }
}
