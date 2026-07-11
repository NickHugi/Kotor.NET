using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class MagnetTemplate
{
    public required Vector3 LocalPosition { get; init; }
    public required Quaternion LocalOrientation { get; init; }
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public string KitID { get; init; } = "";
    public string TemplateID { get; init; } = "";
    public WorldObjectTemplate Template => Kit.Manager.Get(KitID).Object(TemplateID);
}
