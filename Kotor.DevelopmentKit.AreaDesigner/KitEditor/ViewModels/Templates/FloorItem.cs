using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Templates;

public class FloorItem : WorldObjectItem
{
    public override WorldObjectType WorldObjectType => WorldObjectType.Floor;

    public FloorItem() : base()
    {
    }
    public FloorItem(FloorTemplate template) : base(template)
    {
    }

    public override FloorTemplate ToModel()
    {
        return new FloorTemplate
        {
            KitID = KitID,
            TemplateID = TemplateID,
            Name = Name,
            ClassID = ClassID,
            Model = Model,
            Magnets = []
        };
    }
}
