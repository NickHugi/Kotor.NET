using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Templates;

public class InnerCornerItem : WorldObjectItem
{
    public override WorldObjectType WorldObjectType => WorldObjectType.InnerCorner;

    public InnerCornerItem() : base()
    {
    }
    public InnerCornerItem(InnerCornerTemplate template) : base(template)
    {
    }

    public override InnerCornerTemplate ToModel()
    {
        return new InnerCornerTemplate
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
