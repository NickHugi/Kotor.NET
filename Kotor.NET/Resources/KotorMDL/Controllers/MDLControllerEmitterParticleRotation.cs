using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerEmitterParticleRotation : BaseMDLController
{
    public float Rotation { get; set; }

    public MDLControllerEmitterParticleRotation()
    {
    }
    public MDLControllerEmitterParticleRotation(float rotation)
    {
        Rotation = rotation;
    }

    public override string ToString()
    {
        return $"Rotation={Rotation}";
    }
}
