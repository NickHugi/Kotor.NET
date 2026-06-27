using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class CeilingHookItem : HookItem
{
    public override string Name => $"{DefaultTemplateID} ({Position.X:F2}, {Position.Y:F2}, {Position.Z:F2})";
    public string DefaultTemplateID
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public CeilingHookItem() : base()
    {
        DefaultTemplateID = "";

        this.WhenAnyValue(x => x.DefaultTemplateID).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
        this.WhenAnyValue(x => x.Position).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
    }
    public CeilingHookItem(CeilingHookTemplate template) : this()
    {
        Position = new(template.LocalPosition);
        Orientation = new(template.LocalOrientation);
        DefaultTemplateID = template.DefaultTemplateID;
    }

    public CeilingHookTemplate ToModel()
    {
        return new CeilingHookTemplate
        {
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
            DefaultTemplateID = DefaultTemplateID,
        };
    }
}
