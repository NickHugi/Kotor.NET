﻿using IniParser.Model;
using IniParser.Parser;
using KotorDotNET.Exceptions;
using KotorDotNET.FileFormats.Kotor2DA;
using KotorDotNET.FileFormats.KotorSSF;
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

        public LegacyINIReader(string iniText)
        {
            var iniDataParser = new IniDataParser();
            ini = iniDataParser.Parse(iniText);
        }

        public PatcherData Parse()
        {
            var tlkList = ini.Sections.SingleOrDefault(x => x.SectionName == "TLKList");
            // TODO: InstallList
            var twodaList = ini.Sections.SingleOrDefault(x => x.SectionName == "2DAList");
            var gffList = ini.Sections.SingleOrDefault(x => x.SectionName == "GFFList");
            var compileList = ini.Sections.SingleOrDefault(x => x.SectionName == "CompileListList");
            var ssfList = ini.Sections.SingleOrDefault(x => x.SectionName == "SSFList");

            if (twodaList is not null)
            {
                foreach (var pair in twodaList.Keys)
                {
                    var filename = pair.KeyName;
                    var twodaSection = ini[pair.Value];

                    var modifyFile = new ModifyFile<TwoDA>();
                    modifyFile.Modifiers = new TwoDASectionParser(ini, twodaSection).Parse();

                    patcherData.TwoDAFiles.Add(modifyFile);
                }
            }

            if (ssfList is not null)
            {
                foreach (var pair in ssfList.Keys)
                {
                    var filename = pair.KeyName;
                    var ssfSection = ini[pair.Value];

                    var modifyFile = new ModifyFile<SSF>();
                    modifyFile.Modifiers = new SSFSectionParser(ini, ssfSection).Parse();

                    patcherData.SSFFiles.Add(modifyFile);
                }
            }

            return patcherData;
        }
    }
}
