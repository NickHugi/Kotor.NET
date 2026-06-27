using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class DoorFrameItem : ObjectItem
{
    public override WorldObjectType WorldObjectType => WorldObjectType.DoorFrame;

    public ObservableCollection<DoorFrameHookItem> Hooks
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public DoorFrameItem() : base()
    {
        Hooks = [new(), new()];
    }
    public DoorFrameItem(DoorFrameTemplate template) : base(template)
    {
        Hooks = new(template.Hooks.Select(x => new DoorFrameHookItem(x)));
    }

    public DoorFrameTemplate ToModel(string kitID)
    {
        return new DoorFrameTemplate
        {
            KitID = kitID,
            TemplateID = TemplateID,
            Name = Name,
            ClassID = ClassID,
            Model = Model,
            Hooks = Hooks.Select(x => x.ToModel()).ToArray(),
        };
    }
}
