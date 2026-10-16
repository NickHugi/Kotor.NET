using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class WorldObjectItem : ReactiveObject
{
    public IEnumerable<WorldObjectType> WorldObjectTypes { get; } = Enum.GetValues<WorldObjectType>();
    public WorldObjectType Type
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string KitID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string TemplateID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string ClassID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string Name
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string Model
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ObservableCollection<MagnetItem> Magnets { get; init; }
    public MagnetItem? SelectedMagnet
    {
        get;
        set
        {
            if (ReferenceEquals(field, value))
                return;

            this.RaiseAndSetIfChanged(ref field, value);
            this.RaisePropertyChanged(nameof(SelectedMagnetType));
        }
    }
    public MagnetType[] MagnetTypes { get; } = Enum.GetValues<MagnetType>();
    public MagnetType? SelectedMagnetType
    {
        get => SelectedMagnet?.MagnetType;
        set
        {
            if (SelectedMagnet is null || value is null || SelectedMagnet.MagnetType == value.Value)
                return;

            var index = Magnets.IndexOf(SelectedMagnet);
            if (index < 0)
                return;

            var replacement = CreateMagnet(value.Value);
            CopyCommonValues(SelectedMagnet, replacement);
            replacement.MagnetType = value.Value;
            Magnets[index] = replacement;
            SelectedMagnet = replacement;
        }
    }

    public WorldObjectItem()
    {
        KitID = "";
        TemplateID = "";
        Name = "";
        ClassID = "";
        Model = "";
        Magnets = [];
    }
    public WorldObjectItem(WorldObjectTemplate template) : this()
    {
        Type = template.Type;
        KitID = template.KitID;
        TemplateID = template.TemplateID;
        Name = template.Name;
        ClassID = template.ClassID;
        Model = template.Model;
        Magnets =
        [
            ..template.Magnets.Select(MagnetItem.FromModel),
        ];
    }

    public WorldObjectTemplate ToModel()
    {
        return new WorldObjectTemplate
        {
            Type = Type,
            KitID = KitID,
            TemplateID = TemplateID,
            Name = Name,
            ClassID = ClassID,
            Model = Model,
            Magnets = Magnets.Select(x => x.ToModel()).ToArray(),
        };
    }

    public void DeleteSelectedMagnet()
    {
        if (SelectedMagnet is null)
            return;

        Magnets.Remove(SelectedMagnet);
        SelectedMagnet = null;
    }
    public void AddMagnet()
    {
        var magnet = new MagnetItem();
        Magnets.Add(magnet);
        SelectedMagnet = magnet;
    }

    private static MagnetItem CreateMagnet(MagnetType type)
    {
        return type switch
        {
            MagnetType.TargetHook => new TargetHookItem(),
            MagnetType.SourceHook => new SourceHookItem(),
            _ => new MagnetItem(),
        };
    }

    private static void CopyCommonValues(MagnetItem source, MagnetItem target)
    {
        target.Tag = source.Tag;
        target.Position = new(source.Position.ToModel());
        target.Orientation = new(source.Orientation.ToModel());
        target.Scale = new(source.Scale.ToModel());
    }
}
