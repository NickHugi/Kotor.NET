using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Localization;

namespace Kotor.NET.Interfaces;

public interface ITalkTableLookup
{
    TalkTableString Locate(string path, StringRef stringref);
}
