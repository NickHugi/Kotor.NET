using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Templates;

public class OuterCornerItem : WorldObjectItem
{
    public override WorldObjectType WorldObjectType => WorldObjectType.OuterCorner;

    public OuterCornerItem() : base()
    {
    }
    public OuterCornerItem(OuterCornerTemplate template) : base(template)
    {
    }

    public override OuterCornerTemplate ToModel()
    {
        return new OuterCornerTemplate
        {
            Type = WorldObjectType.OuterCorner,
            KitID = KitID,
            TemplateID = TemplateID,
            Name = Name,
            ClassID = ClassID,
            Model = Model,
            Magnets = []
        };
    }
}
