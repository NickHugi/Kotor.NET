using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.FileFormats.KotorGFF;

namespace KotorDotNET.Patching.Modifiers.ForGFF
{
    public class AddFieldGFFModifier : IModifier<GFF>
    {
        public string Label { get; set; }
        public object Value { get; set; }
        /// <summary>
        /// Stores the path to the new field into memory with this ID. If this is
        /// null nothing is stored.
        /// </summary>
        public int? ToStoreTokenID { get; set; }
        public List<AddFieldGFFModifier> Modifiers { get; set; }

        private object? _target;

        public AddFieldGFFModifier(string label, object value)
        {
            Label = label;
            Value = value;
        }

        public void Apply(GFF target, Memory memory, ILogger logger)
        {
            if (_target == null)
            {
                target.Root.Set(Label, Value);
            }
            else if (_target is GFFStruct)
            {
                (_target as GFFStruct)!.Set(Label, Value);
            }
            else if (_target is GFFList)
            {
                if (Value is not GFFStruct)
                {
                    throw new ArgumentException(""); // TODO
                }

                (_target as GFFList)!.Add(Value as GFFStruct);
            }

            foreach (var modifier in Modifiers)
            {
                if (_target is GFFStruct)
                {
                    modifier.SetTarget((GFFStruct)_target);
                    modifier.Apply(target, memory, logger);
                }
                else if (_target is GFFList)
                {
                    modifier.SetTarget((GFFList)_target);
                    modifier.Apply(target, memory, logger);
                }
                else
                {
                    throw new ArgumentException(); // TODO
                }
            }
        }

        public void SetTarget(GFFStruct @struct)
        {
            _target = @struct;
        }
        public void SetTarget(GFFList list)
        {
            _target = list;
        }
    }
}
