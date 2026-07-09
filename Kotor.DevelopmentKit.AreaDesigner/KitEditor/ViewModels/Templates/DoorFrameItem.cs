using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Templates;

public class DoorFrameItem : WorldObjectItem
{
    public override WorldObjectType WorldObjectType => WorldObjectType.DoorFrame;

    public DoorFrameItem() : base()
    {
    }
    public DoorFrameItem(UltimateWorldObjectTemplate template) : base(template)
    {
        Hooks = new(template.Magnets.OfType<UltimateMagnetTemplate>().Select(x => new DoorFrameHookItem(x)));
    }

    public override UltimateWorldObjectTemplate ToModel()
    {
        return new UltimateWorldObjectTemplate
        {
            Type = WorldObjectType.DoorFrame,
            KitID = KitID,
            TemplateID = TemplateID,
            Name = Name,
            ClassID = ClassID,
            Model = Model,
            Magnets = Hooks.Select(x => x.ToModel()).ToArray(),
        };
    }
}
