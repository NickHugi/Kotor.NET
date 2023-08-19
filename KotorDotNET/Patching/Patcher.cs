using KotorDotNET.Common.Data;
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
        public PatcherData PatcherData { get; set; }

        public Patcher(Memory memory, ILogger logger, PatcherData data)
        {
            Memory = memory;
            Logger = logger;
            PatcherData = data;
        }
        
        /// <summary>
        /// Execute the patcher data to the game files.
        /// </summary>
        public void Run()
        {
            foreach (var pair in PatcherData.TwoDAModifiers)
            {
                var reference = pair.Key;
                var modifiers = pair.Value;

                //var twoda = new TwoDABinaryReader(reference.FetchData()).Read();

                //foreach (var modifier in modifiers)
                //{
                //    modifier.Apply(twoda, Memory, Logger);
                //}

                // TODO write output
            }
        }
    }
}
