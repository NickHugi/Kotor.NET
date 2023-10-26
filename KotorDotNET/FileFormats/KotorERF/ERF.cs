using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Common.Data;

namespace KotorDotNET.FileFormats.KotorERF
{
    public class ERF
    {
        public List<Resource> Resources { get; set; }
        public ERFType Type { get; set; }

        public ERF()
        {
            Resources = new();
        }

        public Resource? Get(ResRef ResRef, ResourceType ResType)
        {
            return Resources.FirstOrDefault(x => x.ResRef == ResRef && x.Type == ResType);
        }
    }

    public enum ERFType
    {
        ERF,
        MOD,
        SAV,
    }
}
