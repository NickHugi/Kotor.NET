using IniParser.Model;
using KotorDotNET.Exceptions;
using KotorDotNET.FileFormats.Kotor2DA;
using KotorDotNET.FileFormats.KotorTLK;
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
    public class LegacyINIReader : IPatcherDataReader
    {
        protected IniData ini { get; set; } = new();
        protected TLK tlk { get; set; } = new();
        protected PatcherData patcherData { get; set; } = new();

        public PatcherData Parse()
        {
            var tlkList = ini.Sections.SingleOrDefault(x => x.SectionName == "2DAList");
            var twodaList = ini.Sections.SingleOrDefault(x => x.SectionName == "2DAList");
            var gffList = ini.Sections.SingleOrDefault(x => x.SectionName == "2DAList");
            var compileList = ini.Sections.SingleOrDefault(x => x.SectionName == "2DAList");
            var ssfList = ini.Sections.SingleOrDefault(x => x.SectionName == "2DAList");

            if (twodaList != null)
            {
                foreach (var pair in twodaList.Keys)
                {
                    var filename = pair.KeyName;
                    var twodaSection = ini[pair.Value];
                    var modifiers = new TwoDASectionParser(ini, twodaSection).Parse();
                    var modifyFile = new ModifyFile<TwoDA>();
                    patcherData.TwoDAModifiers[filename] = modifiers;
                }
            }

            return patcherData;
        }
    }
}
