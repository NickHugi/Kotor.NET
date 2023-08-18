using IniParser.Model;
using KotorDotNET.FileFormats.Kotor2DA;
using KotorDotNET.Patching.Modifiers.For2DA;
using KotorDotNET.Patching.Modifiers.For2DA.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching.Parsers.LegacyINI
{
    /// <summary>
    /// For Parsing changes.ini to a Patcher instance following
    /// Stoffe's TSLPatcher specifications.
    /// </summary>
    public class LegacyINIParser : IParser
    {
        protected IniData Data { get; set; }

        public Patcher Read()
        {
            throw new NotImplementedException();
        }

        protected IModifier<TwoDA> ReadEditRowModifier(string sectionKey)
        {
            ITarget target;
            Dictionary<string, IValue> data = null;
            Dictionary<int, IValue> toStoreInMemory = null;

            var section = Data[sectionKey];

            if (section.ContainsKey("RowIndex"))
            {
                // TODO exception
                var rowIndex = int.Parse(section["RowIndex"]);
                target = new RowIndexTarget(rowIndex);
            }
            else if (section.ContainsKey("RowLabel"))
            {
                target = new RowHeaderTarget(section["RowLabel"]);
            }
            else if (section.ContainsKey("LabelIndex"))
            {
                target = new ColumnTarget("label", section["LabelIndex"]);
            }

            /*EditRowModifier modifier = new(target, data, toStoreInMemory);
            return modifier;*/
            return default; // TODO
        }
    }
}
