using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching.Modifiers
{
    public class Logger : ILogger
    {
        public void Error()
        {
            throw new NotImplementedException();
        }

        public void Information()
        {
            throw new NotImplementedException();
        }

        public void Verbose()
        {
            throw new NotImplementedException();
        }

        public void Warning()
        {
            throw new NotImplementedException();
        }
    }
}
