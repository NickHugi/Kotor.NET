using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching
{
    public interface IMemory
    {
        public string? From2DAToken(int tokenID);

        public int? FromTLKToken(int tokenID);

        public void Set2DAToken(int tokenID, string value);

        public void SetTLKToken(int tokenID, int value);

        void Clear();
    }
}
