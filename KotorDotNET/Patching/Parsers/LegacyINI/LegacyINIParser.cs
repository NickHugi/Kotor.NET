using IniParser.Model;
using KotorDotNET.Exceptions;
using KotorDotNET.FileFormats.Kotor2DA;
using KotorDotNET.Patching.Modifiers.For2DA;
using KotorDotNET.Patching.Modifiers.For2DA.Targets;
using KotorDotNET.Patching.Modifiers.For2DA.Values;
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
    public class LegacyINIParser : IPatcherDataParser
    {
        protected IniData ini { get; set; } = new();
        protected PatcherData patcherData { get; set; } = new();

        public PatcherData Parse()
        {
            var twodaList = ini.Sections.SingleOrDefault(x => x.SectionName == "2DAList");

            if (twodaList != null)
            {
                foreach (var pair in twodaList.Keys)
                {
                    var filename = pair.KeyName;
                    var twodaSection = ini[pair.Value];
                    var modifiers = new TwoDASectionParser(ini, twodaSection).Parse();
                    patcherData.TwoDAModifiers[filename] = modifiers;
                }
            }

            return patcherData;
        }
    }
}
