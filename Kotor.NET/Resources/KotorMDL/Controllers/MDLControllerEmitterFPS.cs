using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerEmitterFPS : BaseMDLController
{
    public float FramesPerSecond { get; set; }

    public MDLControllerEmitterFPS()
    {
    }
    public MDLControllerEmitterFPS(float fps)
    {
        FramesPerSecond = fps;
    }

    public override string ToString()
    {
        return $"FPS={FramesPerSecond}";
    }
}
