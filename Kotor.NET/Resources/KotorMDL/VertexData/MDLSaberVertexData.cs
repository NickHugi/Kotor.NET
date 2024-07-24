using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorMDL.VertexData;

public class MDLSaberVertexData
{
    public Vector3 Position { get; set; } = new();
    public Vector3 Normal { get; set; } = new();
    public Vector2 UV { get; set; } = new();
}
