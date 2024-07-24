using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerAlpha : BaseMDLController
{
    public float Alpha { get; set; }

    public MDLControllerAlpha()
    {
    }
    public MDLControllerAlpha(float alpha)
    {
        Alpha = alpha;
    }

    public override string ToString()
    {
        return $"Alpha={Alpha}";
    }
}
