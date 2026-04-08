using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterLifeExpectancy : BaseMDLControllerData
{
    public float Time { get; set; }

    public MDLControllerDataEmitterLifeExpectancy()
    {
    }
    public MDLControllerDataEmitterLifeExpectancy(float time)
    {
        Time = time;
    }

    public override string ToString()
    {
        return $"Time={Time}";
    }
}
