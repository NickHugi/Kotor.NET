using System;
using Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class MagnetItem : ReactiveObject
{
    public virtual string TypeName => "Magnet";
    public string Name => $"{TypeName}: {Tag} ({Position.X:F2}, {Position.Y:F2}, {Position.Z:F2})";

    public string Tag
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public MagnetType MagnetType
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
    public ReactiveVector3 Scale
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public MagnetItem()
    {
        Tag = "";
        Position = new();
        Orientation = new(0, 0, 0, 1);
        Scale = new(1, 1, 1);

        this.WhenAnyValue(x => x.Tag).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
        this.WhenAnyValue(x => x.Position).Subscribe(_ => this.RaisePropertyChanged(nameof(Name)));
    }

    public MagnetItem(MagnetTemplate template) : this()
    {
        Tag = template.Tag;
        Position = new ReactiveVector3(template.LocalPosition);
        Orientation = new ReactiveQuaternion(template.LocalOrientation);
        Scale = new ReactiveVector3(template.LocalScale);
        MagnetType = template.MagnetType;
    }

    public static MagnetItem FromModel(MagnetTemplate template)
    {
        return template switch
        {
            TargetHookTemplate targetHook => new TargetHookItem(targetHook),
            SourceHookTemplate sourceHook => new SourceHookItem(sourceHook),
            _ => new MagnetItem(template),
        };
    }

    public virtual MagnetTemplate ToModel()
    {
        return new MagnetTemplate
        {
            Tag = Tag,
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
            LocalScale = Scale.ToModel(),
            MagnetType = MagnetType,
        };
    }
}
