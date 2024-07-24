using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Nodes;

public class MDLEmitterNode : MDLNode
{
    public float DeadSpace { get; set; }
    public float BlastRadius { get; set; }
    public float BlastLength { get; set; }
    public uint BranchCount { get; set; }
    public float ControlPointSmoothing { get; set; }
    public int XGrid { get; set; }
    public int YGrid { get; set; }
    public int SpawnType { get; set; }
    public string Update { get; set; } = "";
    public string Render { get; set; } = "";
    public string Blend { get; set; } = "";
    public string Texture { get; set; } = "";
    public string ChunkName { get; set; } = "";
    public uint TwoSidedTexture { get; set; }
    public uint Loop { get; set; }
    public ushort RenderOrder { get; set; }
    public byte FrameBlending { get; set; }
    public string DepthTextureName { get; set; } = "";
    public bool P2P { get; set; }
    public bool P2P_SEL { get; set; }
    public bool AffectedByWind { get; set; }
    public bool Tinted { get; set; }
    public bool Bounce { get; set; }
    public bool Random { get; set; }
    public bool Inherit { get; set; }
    public bool InheritVelocity { get; set; }
    public bool InheritLocal { get; set; }
    public bool Splat { get; set; }
    public bool InheritPart { get; set; }
    public bool DepthTexture { get; set; }
    public bool Flag13 { get; set; }

    public MDLEmitterNode(string name) : base(name)
    {
    }
}
