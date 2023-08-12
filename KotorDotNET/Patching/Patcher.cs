using KotorDotNET.Common.Data;
using KotorDotNET.Common.FileFormats.Kotor2DA;
using KotorDotNET.FileFormats.Kotor2DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching
{
    public class Patcher
    {
        public Memory Memory { get; set; }
        public ILogger Logger { get; set; }
        public Dictionary<ResourceReference, List<IModifier<TwoDA>>> TwoDAModifiers { get; set; }

        public Patcher(Memory memory, ILogger logger)
        {
            Memory = memory;
            Logger = logger;
            TwoDAModifiers = new();
        }

        // TODO
        //public List<IModifier<TLK>> TLKModifiers = new();
        //public List<IModifier<SSF>> TLKModifiers = new();
        //public List<IModifier<GFF>> TLKModifiers = new();
        //public List<FileOperation> FileOperations = new();
        
        /// <summary>
        /// Execute the patcher data to the game files.
        /// </summary>
        public void Run()
        {
            foreach (var pair in TwoDAModifiers)
            {
                var reference = pair.Key;
                var modifiers = pair.Value;

                var twoda = new TwoDABinaryReader(reference.FetchData()).Read();

                foreach (var modifier in modifiers)
                {
                    modifier.Apply(twoda, Memory, Logger);
                }

                // TODO write output
            }
        }
    }
}
