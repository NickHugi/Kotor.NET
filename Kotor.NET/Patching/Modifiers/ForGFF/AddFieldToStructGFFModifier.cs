﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Exceptions;
using Kotor.NET.Formats.KotorGFF;
using Kotor.NET.Patching.Modifiers.ForGFF.Values;

namespace Kotor.NET.Patching.Modifiers.ForGFF
{
    public class AddFieldToStructGFFModifier : IGFFModifier
    {
        internal GFFNavigator? _navigator;

        public string Label { get; set; }
        public object Value { get; set; }
        public GFFFieldType FieldType { get; set; }
        public string Path { get; set; }
        public Dictionary<int, IValue> ToStoreInMemory { get; set; }
        public List<IGFFModifier> SubFields { get; set; }

        public AddFieldToStructGFFModifier(string label, object value, GFFFieldType fieldType, string path, Dictionary<int, IValue> toStoreInMemory, List<IGFFModifier> subFields)
        {
            Label = label;
            Value = value;
            FieldType = fieldType;
            Path = path;
            ToStoreInMemory = toStoreInMemory;
            SubFields = subFields;
        }

        public void Apply(GFF target, IMemory memory, ILogger logger)
        {
            if (_navigator == null)
                _navigator = new GFFNavigator(target.Root);

            var nodes = Path.Split("\\", StringSplitOptions.RemoveEmptyEntries);
            foreach (var node in nodes)
            {
                _navigator.NavigateTo(node);
            }

            var position = _navigator.Resolve();
            if (position is GFFStruct gffStruct)
            {
                gffStruct.Set(Label, Value);
            }
            else
            {
                throw new PatchingParserException("Specified path did not lead to a struct.");
            }

            foreach (var toStoreInMemory in ToStoreInMemory)
            {
                var tokenID = toStoreInMemory.Key;
                var value = toStoreInMemory.Value.Resolve(memory, target);
                memory.Set2DAToken(tokenID, value);
            }

            foreach (var subfield in SubFields)
            {
                subfield.SetNavigator(_navigator);
                subfield.Apply(target, memory, logger);
            }
        }

        public void SetNavigator(GFFNavigator navigator)
        {
            _navigator = navigator;
        }
    }
}
