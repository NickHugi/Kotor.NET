using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;

public class CornerHookTemplate : BaseMagnetTemplate
{
    public required string InnerKitID { get; init; }
    public required string InnerTemplateID { get; init; }
    public InnerCornerTemplate InnerTemplate => Kit.Manager.Get(InnerKitID).InnerCorner(InnerTemplateID);

    public required string OuterKitID { get; init; }
    public required string OuterTemplateID { get; init; }
    public OuterCornerTemplate OuterCornerTemplate => Kit.Manager.Get(OuterKitID).OuterCorner(OuterTemplateID);

    public required int[] Adjacent { get; init; }
}
