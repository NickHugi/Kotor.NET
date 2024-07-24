using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerEmitterLifeExpectancy : BaseMDLController
{
    public float Time { get; set; }

    public MDLControllerEmitterLifeExpectancy()
    {
    }
    public MDLControllerEmitterLifeExpectancy(float time)
    {
        Time = time;
    }

    public override string ToString()
    {
        return $"Time={Time}";
    }
}
