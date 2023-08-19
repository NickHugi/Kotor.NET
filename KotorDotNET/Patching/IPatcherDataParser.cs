using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching
{
    /// <summary>
    /// Parses data stored in some sort of format (such as the
    /// changes.ini file used by TSLPatcher) and converts it to a 
    /// Patcher instance that can be executed.
    /// </summary>
    public interface IPatcherDataParser
    {
        PatcherData Parse();
    }
}
