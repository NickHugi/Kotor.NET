using KotorDotNET.Common.Data;
using KotorDotNET.FileFormats.Kotor2DA;
using KotorDotNET.FileFormats.KotorGFF;
using KotorDotNET.FileFormats.KotorSSF;
using KotorDotNET.FileFormats.KotorTLK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching
{
    public class PatcherData
    {
        public ModifyFile<TwoDA> TwoDAFiles { get; set; } = new();
        public ModifyFile<GFF> GFFFiles { get; set; } = new();
        public ModifyFile<SSF> SSFFiles { get; set; } = new();
        public ModifyFile<TLK> TLKFiles { get; set; } = new();
    }
}
