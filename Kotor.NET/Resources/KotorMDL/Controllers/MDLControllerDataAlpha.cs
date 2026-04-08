using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataAlpha : BaseMDLControllerData
{
    public float Alpha { get; set; }

    public MDLControllerDataAlpha()
    {
    }
    public MDLControllerDataAlpha(float alpha)
    {
        Alpha = alpha;
    }

    public override string ToString()
    {
        return $"Alpha={Alpha}";
    }
}
