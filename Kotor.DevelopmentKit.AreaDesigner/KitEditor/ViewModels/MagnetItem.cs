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

    public bool ConditionCheckLocalMagnetsOnly
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public bool ConditionMustHaveTemplate
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public bool ConditionOverlapWillDisable
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public int ConditionOverlapCheckCount
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public OverlapCountType ConditionOverlapType
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public bool ConditionOverlapOnlyEnableFirst
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public bool ConditionOverlapOnlyEnableMiddle
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public bool ConditionOverlapOnlySameTemplate
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public bool ConditionOverlapOnlySameClass
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public bool ConditionOverlapOnlySameType
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public WorldObjectType?[]? ConditionOverlapOnlySpecificTypes
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
    public MagnetItem(MagnetTemplate template) : this()
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;
        Position = new ReactiveVector3(template.LocalPosition);
        Orientation = new ReactiveQuaternion(template.LocalOrientation);
        ConditionCheckLocalMagnetsOnly = template.ConditionCheckLocalMagnetsOnly;
        ConditionMustHaveTemplate = template.ConditionMustHaveTemplate;
        ConditionOverlapWillDisable = template.ConditionOverlapWillDisable;
        ConditionOverlapCheckCount = template.ConditionOverlapCheckCount;
        ConditionOverlapType = template.ConditionOverlapType;
        ConditionOverlapOnlyEnableFirst = template.ConditionOverlapOnlyEnableFirst;
        ConditionOverlapOnlyEnableMiddle = template.ConditionOverlapOnlyEnableMiddle;
        ConditionOverlapOnlySameTemplate = template.ConditionOverlapOnlySameTemplate;
        ConditionOverlapOnlySameClass = template.ConditionOverlapOnlySameClass;
        ConditionOverlapOnlySameType = template.ConditionOverlapOnlySameType;
        ConditionOverlapOnlySpecificTypes = template.ConditionOverlapOnlySpecificTypes;
    }

    public MagnetTemplate ToModel()
    {
        return new MagnetTemplate
        {
            KitID = KitID,
            TemplateID = TemplateID,
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
            ConditionCheckLocalMagnetsOnly = ConditionCheckLocalMagnetsOnly,
            ConditionMustHaveTemplate = ConditionMustHaveTemplate,
            ConditionOverlapWillDisable = ConditionOverlapWillDisable,
            ConditionOverlapCheckCount = ConditionOverlapCheckCount,
            ConditionOverlapType = ConditionOverlapType,
            ConditionOverlapOnlyEnableFirst = ConditionOverlapOnlyEnableFirst,
            ConditionOverlapOnlyEnableMiddle = ConditionOverlapOnlyEnableMiddle,
            ConditionOverlapOnlySameTemplate = ConditionOverlapOnlySameTemplate,
            ConditionOverlapOnlySameClass = ConditionOverlapOnlySameClass,
            ConditionOverlapOnlySameType = ConditionOverlapOnlySameType,
            ConditionOverlapOnlySpecificTypes = ConditionOverlapOnlySpecificTypes
        };
    }
}
