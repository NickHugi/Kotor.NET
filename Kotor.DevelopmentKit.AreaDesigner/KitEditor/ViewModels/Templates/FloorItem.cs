using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Templates;

public class FloorItem : WorldObjectItem
{
    public override WorldObjectType WorldObjectType => WorldObjectType.Floor;

    public FloorItem() : base()
    {
    }
    public FloorItem(UltimateWorldObjectTemplate template) : base(template)
    {
    }

    public override UltimateWorldObjectTemplate ToModel()
    {
        return new UltimateWorldObjectTemplate
        {
            Type = WorldObjectType.Floor,
            KitID = KitID,
            TemplateID = TemplateID,
            Name = Name,
            ClassID = ClassID,
            Model = Model,
            Magnets = []
        };
    }
}
