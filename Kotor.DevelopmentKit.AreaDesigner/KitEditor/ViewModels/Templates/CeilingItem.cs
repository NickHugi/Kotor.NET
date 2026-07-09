using System.Collections.ObjectModel;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Templates;

public class CeilingItem : WorldObjectItem
{
    public override WorldObjectType WorldObjectType =>  WorldObjectType.Ceiling;

    public CeilingItem() : base()
    {
    }
    public CeilingItem(UltimateWorldObjectTemplate template) : base(template)
    {
    }

    public override UltimateWorldObjectTemplate ToModel()
    {
        return new UltimateWorldObjectTemplate
        {
            Type = WorldObjectType.Ceiling,
            KitID = KitID,
            TemplateID = TemplateID,
            Name = Name,
            ClassID = ClassID,
            Model = Model,
            Magnets = []
        };
    }
}
