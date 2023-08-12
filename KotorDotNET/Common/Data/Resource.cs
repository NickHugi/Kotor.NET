using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Common.Data
{
    public class Resource
    {
        public ResRef ResRef { get; private set; }
        public ResourceType Type { get; private set; }
        public byte[] Data { get; private set; }

        public Resource(ResRef resRef, ResourceType type, byte[] data)
        {
            ResRef = resRef;
            Type = type;
            Data = data;
        }
    }
}
