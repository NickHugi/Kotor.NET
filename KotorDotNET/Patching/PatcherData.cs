using KotorDotNET.Common.Data;
using KotorDotNET.FileFormats.Kotor2DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching
{
    public class PatcherData
    {
        public Dictionary<string, List<IModifier<TwoDA>>> TwoDAModifiers { get; set; } = new();

        // TODO
        //public List<FileOperation> FileOperations = new();
        //public List<IModifier<TLK>> TLKModifiers = new();
        //public Dictionary<string, IModifier<SSF>> TLKModifiers = new();
        //public Dictionary<string, IModifier<GFF>> TLKModifiers = new();
    }
}
