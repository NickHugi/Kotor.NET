using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using ReactiveUI;
using MagnetModelType = Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff.MagnetType;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Hooks;

public class SourceHookItem : MagnetItem
{
    public override string TypeName => "Target Hook";

    public string KitID
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string C1TemplateID
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string C2TemplateID
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string C3TemplateID
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string C4TemplateID
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public SourceHookItem()
    {
        MagnetType = MagnetModelType.SourceHook;
        KitID = "";
        C1TemplateID = "";
        C2TemplateID = "";
        C3TemplateID = "";
        C4TemplateID = "";
    }

    public SourceHookItem(SourceHookTemplate template) : base(template)
    {
        MagnetType = MagnetModelType.SourceHook;
        KitID = template.KitID;
        C1TemplateID = template.C1TemplateID;
        C2TemplateID = template.C2TemplateID;
        C3TemplateID = template.C3TemplateID;
        C4TemplateID = template.C4TemplateID;
    }

    public override MagnetTemplate ToModel()
    {
        return new SourceHookTemplate
        {
            Tag = Tag,
            KitID = KitID,
            C1TemplateID = C1TemplateID,
            C2TemplateID = C2TemplateID,
            C3TemplateID = C3TemplateID,
            C4TemplateID = C4TemplateID,
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
            LocalScale = Scale.ToModel(),
            MagnetType = MagnetModelType.SourceHook,
        };
    }
}
