using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching
{
    public interface ILogger
    {
        void Verbose();

        void Information();
        
        void Warning();

        void Error();
    }
}
