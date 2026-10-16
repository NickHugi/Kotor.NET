using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using ReactiveUI;
using MagnetModelType = Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff.MagnetType;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Hooks;

public class TargetHookItem : MagnetItem
{
    public override string TypeName => "Target Hook";

    public TargetHookItem()
    {
        MagnetType = MagnetModelType.SourceHook;
    }

    public TargetHookItem(TargetHookTemplate template) : base(template)
    {
        MagnetType = MagnetModelType.SourceHook;
    }

    public override MagnetTemplate ToModel()
    {
        return new TargetHookTemplate
        {
            Tag = Tag,
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
            LocalScale = Scale.ToModel(),
            MagnetType = MagnetModelType.TargetHook,
        };
    }
}
