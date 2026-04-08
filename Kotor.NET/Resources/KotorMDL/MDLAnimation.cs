using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorMDL.Nodes;

namespace Kotor.NET.Resources.KotorMDL;

public class MDLAnimation
{
    public string Name { get; set; } = "";
    public string AnimationRoot { get; set; } = "";
    public float AnimationLength { get; set; }
    public float TransitionTime { get; set; }

    public MDLNode RootNode { get; set; } = new("root");
    public List<MDLAnimationEvent> Events = new();
}
