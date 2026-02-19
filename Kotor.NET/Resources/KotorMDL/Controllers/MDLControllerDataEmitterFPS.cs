using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterFPS : BaseMDLControllerData
{
    public float FramesPerSecond { get; set; }

    public MDLControllerDataEmitterFPS()
    {
    }
    public MDLControllerDataEmitterFPS(float fps)
    {
        FramesPerSecond = fps;
    }

    public override string ToString()
    {
        return $"FPS={FramesPerSecond}";
    }
}
