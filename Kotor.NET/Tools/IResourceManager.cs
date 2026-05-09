using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorMDL;
using Kotor.NET.Resources.KotorTLK;
using Kotor.NET.Resources.KotorTPC;
using Kotor.NET.Resources.KotorUTC;
using Kotor.NET.Resources.KotorUTD;

namespace Kotor.NET.Tools;

public interface IResourceManager
{
    public TPC GetTexture(ResRef resref);
}
