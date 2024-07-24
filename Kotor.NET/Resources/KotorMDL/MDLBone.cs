using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorMDL;

public class MDLBone
{
    public int Bonemap { get; set; } = -1; // TODO - bone limit 16/17?
    public Vector3 TBone { get; set; } = new();
    public Vector4 QBone { get; set; } = new();
}
