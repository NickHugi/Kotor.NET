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
        public IMemory Memory { get; set; }
        public ILogger Logger { get; set; }
        public PatcherData PatcherData { get; set; }

        public Patcher(IMemory memory, ILogger logger, PatcherData data)
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
            
        }
    }
}
