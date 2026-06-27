using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class OuterCornerItem : ObjectItem
{
    public override WorldObjectType WorldObjectType => WorldObjectType.OuterCorner;

    public OuterCornerItem() : base()
    {
    }
    public OuterCornerItem(OuterCornerTemplate template) : base()
    {
    }

    public OuterCornerTemplate ToModel(string kitID)
    {
        return new OuterCornerTemplate
        {
            KitID = kitID,
            TemplateID = TemplateID,
            Name = Name,
            ClassID = ClassID,
            Model = Model,
        };
    }
}
