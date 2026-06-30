using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;

public class HookTemplate : BaseMagnetTemplate
{
    public required string KitID { get; init; }
    public required string TemplateID { get; init; }
}
