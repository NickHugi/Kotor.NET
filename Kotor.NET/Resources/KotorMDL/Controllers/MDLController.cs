using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLController<T> where T : BaseMDLControllerRow<BaseMDLControllerData>
{
    public List<T> Rows = new();
}
