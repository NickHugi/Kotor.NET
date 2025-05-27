using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics.Model;

public class MeshNode : BaseNode
{
    public required IVertexArrayObject VAO { get; set; }
    public required string Texture1 { get; set; }
    public required string Texture2 { get; set; }
}
