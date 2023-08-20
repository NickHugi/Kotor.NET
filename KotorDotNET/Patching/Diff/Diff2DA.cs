using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.FileFormats.Kotor2DA;

namespace KotorDotNET.Patching.Diff
{
    public class Diff2DA
    {
        public TwoDA Older { get; }
        public TwoDA Newer { get; }

        public Diff2DA(TwoDA older, TwoDA newer)
        {
            Older = older;
            Newer = newer;
        }

        public List<IModifier<TwoDA>> Find()
        {
            var modifiers = new List<IModifier<TwoDA>>();

            // TODO

            return modifiers;
        }

        public bool IsSame()
        {
            var oldColumnHeaders = new HashSet<string>(Older.ColumnHeaders());
            var newColumnHeaders = new HashSet<string>(Newer.ColumnHeaders());

            if (!oldColumnHeaders.SetEquals(newColumnHeaders))
            {
                return false;
            }

            if (Older.Rows().Count != Newer.Rows().Count)
            {
                return false;
            }

            foreach (var oldRow in Older.Rows())
            {
                var newRow = Newer.Row(oldRow.Index);

                foreach (var columnHeader in Older.ColumnHeaders())
                {
                    var oldValue = oldRow.GetCell(columnHeader);
                    var newValue = newRow.GetCell(columnHeader);

                    if (oldValue != newValue)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
