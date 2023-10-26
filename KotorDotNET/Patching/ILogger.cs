using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching
{
    public interface ILogger
    {
        void Verbose(string message);

        void Information(string message);
        
        void Warning(string message);

        void Error(string message);
    }
}
