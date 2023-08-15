using IniParser;
using IniParser.Model;
using KotorDotNET.FileFormats.Kotor2DA;
using KotorDotNET.Patching.Modifiers.For2DA;
using KotorDotNET.Patching.Modifiers.For2DA.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Patching.Modifiers;

namespace KotorDotNET.Patching.Parsers.LegacyINI
{
    /// <summary>
    /// For Parsing changes.ini to a Patcher instance following
    /// Stoffe's TSLPatcher specifications.
    /// </summary>
    public class LegacyINIParser : IParser
    {
        public LegacyINIParser(IniData data)
        {
            Data = data;
        }

        protected IniData Data { get; set; }

        // read the namespaces.ini file and if it exists determine all the changes.ini files that needs to be read
        // Read the changes.ini using ini-parser library.
        // Use recursive approach to nest dictionaries based on section headers found as keys in parent sections.
        // Loop through nested dictionary and create necessary types immediately based on top level section names and keys.
        // OR, potentially.
        // Determine what key is being parsed and create the necessary types immediately.
        // Somehow add that information to a Memory object and create a PatchLogger (actually just a Logger object here)
        public Patcher Read()
        {
            //TODO: read tslpatchdata dir elsewhere
            DirectoryInfo tslpatchdata = new (Assembly.GetExecutingAssembly().Location);
            // Check if the namespaces.ini exists
            var namespaceIni = new FileInfo(Path.Combine(tslpatchdata.Name, "namespaces.ini"));
            FileIniDataParser parser = new();

            bool namespaceIniUsed = false;
            if (namespaceIni.Exists)
            {
                IniData namespaceRawIniData = parser.ReadFile(namespaceIni.FullName);

                // Build the nested dictionary starting from the 'Namespaces' top-level section
                Dictionary<string, object> namespaceDictionary = BuildNestedDict(namespaceRawIniData, "Namespaces");
                List<string> changesFiles = GetChangesFilesFromNamespace(namespaceDictionary);

                // assume namespaces.ini isn't relevant if no options are listed there.
                if (changesFiles.Count <= 0)
                {
                    namespaceIniUsed = true;
                }

                // ensure each changes ini referenced by namespaces.ini exists on disk
                for (int i = 0; i < changesFiles.Count; i++)
                {
                    string thisChangesIni = changesFiles[i];
                    if (!File.Exists(thisChangesIni))
                    {
                        throw new FileNotFoundException(
                            $"'{thisChangesIni}' referenced in namespaces.ini option {i} does not exist on disk."
                        );
                    }
                }
            }

            // look for changes.ini in tslpatchdata
            if (!namespaceIniUsed)
            {
                IniData namespaceRawIniData = parser.ReadFile(namespaceIni.FullName);

                // Build the nested dictionary starting from the 'Namespaces' top-level section
                Dictionary<string, object> namespaceDictionary = BuildNestedDict(namespaceRawIniData, new List<string>{"Namespaces"});
                List<string> changesFiles = GetChangesFilesFromNamespace(namespaceDictionary);
                if (changesFiles.Count <= 0)
                {
                    throw new FileLoadException("Could not find any options from this changes.ini!");
                }
                // do something with changes.ini to create a patcher object.
            }
            return new Patcher(new Memory(), new Logger());
        }

        public List<string> GetChangesFilesFromNamespace(Dictionary<string, object> namespaceDictionary)
        {

            List<string> changesFiles = new();
            
            foreach (KeyValuePair<string, object> namespaceEntry in namespaceDictionary)
            {
                string? sectionName = namespaceEntry.Value.ToString();

                // Assuming that each section in the nested dictionary is of type Dictionary<string, object>
                if (sectionName is null || namespaceDictionary[sectionName] is not Dictionary<string, object> sectionDict)
                {
                    throw new FileLoadException("Namespace option missing from header");
                }

                if (!sectionDict.ContainsKey("IniName"))
                {
                    throw new FileLoadException(
                        $"Namespace option '{sectionName}' missing IniName field."
                        + $" Should contain the name of the changes.ini for this option."
                    );
                }

                if (!sectionDict.ContainsKey("IniName"))
                {
                    throw new FileLoadException(
                        $"Namespace option '{sectionName}' missing IniName field."
                        + $" Should contain the name of the changes.ini for this option."
                    );
                }
                
                string? iniNameValue = sectionDict["IniName"].ToString();
                if (iniNameValue is null)
                {
                    continue;
                }

                changesFiles.Add(iniNameValue);
            }

            return changesFiles;
        }


        private static Dictionary<string, object> BuildNestedDict(IniData data, List<string> topLevelSections)
        {
            Dictionary<string, object> result = new();

            foreach (string sectionName in topLevelSections)
            {
                if (!data.Sections.ContainsSection(sectionName))
                    continue;

                KeyDataCollection? section = data[sectionName];
                Dictionary<string, object> nestedDict = new();

                foreach (KeyData? keyData in section)
                {
                    string key = keyData.KeyName;
                    string keyValue = keyData.Value;

                    if (data.Sections.ContainsSection(keyValue))
                    {
                        // Recursively build the dictionary for the nested section
                        nestedDict[key] = BuildNestedDict(data, new List<string> { keyValue });
                    }
                    else
                    {
                        nestedDict[key] = keyValue;
                    }
                }

                result[sectionName] = nestedDict;
            }

            return result;
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

            EditRowModifier modifier = new(target, data, toStoreInMemory);
            return modifier;
        }
    }
}
