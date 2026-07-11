using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class MagnetItem : ReactiveObject
{
    public string Name => $"{KitID}.{TemplateID} ({Position.X:F2}, {Position.Y:F2}, {Position.Z:F2})";

    public string KitID
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string TemplateID
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ReactiveVector3 Position
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public ReactiveQuaternion Orientation
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public MagnetItem()
    {
        KitID = "";
        TemplateID = "";

        Position = new();
        Orientation = new();

        this.WhenAnyValue(x => x.TemplateID).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
        this.WhenAnyValue(x => x.Position).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
    }
    public MagnetItem(UltimateMagnetTemplate template) : this()
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;
        Position = new ReactiveVector3(template.LocalPosition);
        Orientation = new ReactiveQuaternion(template.LocalOrientation);
    }

    public UltimateMagnetTemplate ToModel()
    {
        return new UltimateMagnetTemplate
        {
            KitID = KitID,
            TemplateID = TemplateID,
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
        };
    }
}
