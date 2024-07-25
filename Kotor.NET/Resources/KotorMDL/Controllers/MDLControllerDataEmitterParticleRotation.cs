using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterParticleRotation : BaseMDLControllerData
{
    public float Rotation { get; set; }

    public MDLControllerDataEmitterParticleRotation()
    {
    }
    public MDLControllerDataEmitterParticleRotation(float rotation)
    {
        Rotation = rotation;
    }

    public override string ToString()
    {
        return $"Rotation={Rotation}";
    }
}
