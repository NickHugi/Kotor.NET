using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class InnerCornerItem : ObjectItem
{
    public override WorldObjectType WorldObjectType => WorldObjectType.InnerCorner;

    public InnerCornerItem() : base()
    {
    }
    public InnerCornerItem(InnerCornerTemplate template) : base(template)
    {
    }

    public InnerCornerTemplate ToModel(string kitID)
    {
        return new InnerCornerTemplate
        {
            KitID = kitID,
            TemplateID = TemplateID,
            Name = Name,
            ClassID = ClassID,
            Model = Model,
        };
    }
}
