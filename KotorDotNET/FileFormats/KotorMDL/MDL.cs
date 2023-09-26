using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Common.Geometry;

namespace KotorDotNET.FileFormats.KotorMDL
{
    public class MDL
    {
        public byte ModelType { get; set; } = 0;
        public bool DisableFog { get; set; } = false;
        public float AnimationScale { get; set; } = 0;
        public string Supermodel { get; set; } = "";
        public string ModelName { get; set; } = "";
        public Node Node { get; set; } = new();
        public List<Animation> Animations { get; set; } = new();

        // BoundingBox
        // Radius
    }

    public class Node
    {
        public string Name { get; set; } = "";
        public Vector3 Position { get; set; } = new();
        public Vector4 Rotation { get; set; } = new();
        public List<Node> Children { get; set; } = new();
        public List<Controller> Controllers { get; set; } = new();
        public Trimesh Trimesh = new();
    }

    public class Controller
    {

    }

    public class Trimesh
    {
        public List<Face> Faces { get; set; } = new();
        public Color DiffuseColor { get; set; } = new();
        public Color AmbientColor { get; set; } = new();
        public uint TransperencyHit { get; set; } = 0;
        public string DiffuseTexture { get; set; } = "";
        public string LightmapTexture { get; set; } = "";
        public int SaberValue1 { get; set; } = 0;
        public int SaberValue2 { get; set; } = 0;

        public bool Render { get; set; }
        public bool Shadow { get; set; }
        public bool Beaming { get; set; }
        public bool Lightmap { get; set; }
        public bool RotateTexture { get; set; }
        public bool BackgroundGeometry { get; set; }
        public bool AnimateUV { get; set; }

        public Vector2 UVDirection { get; set; }
        public float UVSpeed { get; set; }
        public float UVJitter { get; set; }


        // BoundingBox
        // Radius
        // Average
        // Total Area
    }

    public class Danglymesh
    {
        public float Displacement { get; set; } = 0;
        public float Tightness { get; set; } = 0;
        public float Period { get; set; } = 0;
    }

    public class Skinmesh
    {

    }

    public class Sabermesh
    {

    }

    public class Light
    {
        public float FlareRadius { get; set; } = 0;
        public List<LightLensFlare> FlareLens { get; set; } = new();
        public int LightPriority { get; set; } = 0;
        public int AmbientOnly { get; set; } = 0;
        public int DynamicType { get; set; } = 0;
        public int AffectDynamic { get; set; } = 0;
        public int Shadow { get; set; } = 0;
        public int Flare { get; set; } = 0;
        public int FadingLight { get; set; } = 0;
    }

    public class LightLensFlare
    {
        public float Size { get; set; } = 0;
        public string Texture { get; set; } = "";
        public float Position { get; set; } = 0;
        public Color ColorShift { get; set; } = new();
    }

    public class Emitter
    {
        public float DeadSpace { get; set; } = 0;
        public float BlastRadius { get; set; } = 0;
        public float BlastLength { get; set; } = 0;
        public int BranchCount { get; set; } = 0;
        public float ControlPointSmoothing { get; set; } = 0;
        public int GridX { get; set; } = 0;
        public int GridY { get; set; } = 0;
        public string Update { get; set; } = "";
        public string Render { get; set; } = "";
        public string Blend { get; set; } = "";
        public string Texture { get; set; } = "";
        public string ChunkName { get; set; } = "";
        public int TwoSidedTexture { get; set; } = 0;
        public int Loop { get; set; } = 0;
        public int RenderOrder { get; set; } = 0;
        public int FrameBlender { get; set; } = 0;
        public string DepthTexture { get; set; } = "";
        public int Flags { get; set; }
    }

    public class Reference
    {
        public string ModelName { get; set; } = "";
        public int Reattachable { get; set; } = 0;
    }

    public class Vector
    {
        public Vector3 Position { get; set; } = new();
        public Vector3 Normal { get; set; } = new();
        public Vector2 UV1 { get; set; } = new();
        public Vector2 UV2 { get; set; } = new();
        public Vector2 UV3 { get; set; } = new();
        public Vector2 UV4 { get; set; } = new();
        public Vector2 Colors { get; set; } = new();
        public Vector2 Tangent1 { get; set; } = new();
        public Vector2 Tangent2 { get; set; } = new();
        public Vector2 Tangent3 { get; set; } = new();
        public Vector2 Tangent4 { get; set; } = new();

        public float DanglyConstraint { get; set; } = 0;
        public Vector3 Dangly { get; set; } = new();
    }

    public class Animation
    {
        public string Name { get; set; } = "";
        public byte GeometryType { get; set; } = 0;
        public float AnimationLength { get; set; } = 0;
        public float TransitionTime { get; set; } = 0;
        public string AnimationRoot { get; set; } = "";
        public List<Event> Events { get; set; } = new();
        public Node Node { get; set; } = new();
    }

    public class Event
    {
        public float ActivationTime { get; set; }
        public string Name { get; set; }
    }
}
