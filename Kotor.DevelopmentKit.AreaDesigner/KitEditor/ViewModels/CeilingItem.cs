using System.Collections.ObjectModel;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class CeilingItem : ObjectItem
{
    public override WorldObjectType WorldObjectType =>  WorldObjectType.Ceiling;

    public CeilingItem() : base()
    {
    }
    public CeilingItem(CeilingTemplate template) : base(template)
    {
    }

    public CeilingTemplate ToModel(string kitID)
    {
        return new CeilingTemplate
        {
            KitID = kitID,
            TemplateID = TemplateID,
            Name = Name,
            ClassID = ClassID,
            Model = Model
        };
    }
}
