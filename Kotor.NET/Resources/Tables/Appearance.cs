namespace Kotor.NET.Resources.Tables;

public class Appearance
{
    public TwoDA Source { get; set; }

    public Appearance()
    {
        Source = new();
    }
    public Appearance(TwoDA source)
    {
        Source = source;
    }
}

public class AppearanceEntry
{
    public TwoDARow SourceRow;

    public AppearanceEntry(TwoDARow sourceRow)
    {
        SourceRow = sourceRow;
    }

    /// <summary>
    /// Fallback name of the appearance if the StringRef property is unused.
    /// </summary>
    /// <remarks>
    /// The label column in 2DA.
    /// </remarks>
    public string Label
    {
        get => SourceRow.GetCell("label")?.AsString() ?? "";
        set => SourceRow.GetCell("label")?.SetString(value);
    }

    /// <summary>
    /// StringRef for the name of the appearance.
    /// </summary>
    /// <remarks>
    /// The string_ref column in 2DA.
    /// </remarks>
    public int? StringRef
    {
        get => SourceRow.GetCell("label")?.AsInt() ?? -1;
        set => SourceRow.GetCell("label")?.SetInt(value);
    }

    /// <summary>
    /// The default model to be use should the clothing-specific cells in the row be unfilled.
    /// </summary>
    /// <remarks>
    /// The race column in 2DA.
    /// </remarks>
    public string DefaultModel
    {
        get => SourceRow.GetCell("race")?.AsString() ?? "";
        set => SourceRow.GetCell("race")?.SetString(value);
    }

    /// <summary>
    /// The distance the creature will have to traverse to complete one cycle of its walking animation. Larger values
    /// will make the creature move more slowly.
    /// </summary>
    /// <remarks>
    /// The walkdist column in 2DA.
    /// </remarks>
    public float WalkDist { get; set; } = 0;

    /// <summary>
    /// The distance the creature will have to traverse to complete one cycle of its walking animation. Larger values
    /// will make the creature move more slowly.
    /// </summary>
    /// <remarks>
    /// The rundist column in 2DA.
    /// </remarks>
    public float RunDist { get; set; } = 0;

    /// <summary>
    /// The default texture the default model should use.
    /// </summary>
    /// <remarks>
    /// The racetext column in 2DA.
    /// </remarks>
    public string DefaultTexture { get; set; } = "";

    /// <summary>
    /// Determines what different parts of the creature is rendered.
    /// </summary>
    /// <remarks>
    /// The modeltype column in 2DA.
    /// </remarks>
    public AppearanceModelType ModelType { get; set; }

    /// <summary>
    /// The default head model to use when applicable.
    /// </summary>
    /// <remarks>
    /// The normalhead column in 2DA.
    /// </remarks>
    public string DefaultHead { get; set; }

    /// <summary>
    /// An alternative head model that is used when the default head is already in use by the PC.
    /// </summary>
    /// <remarks>
    /// The backuphead column in 2DA.
    /// </remarks>
    public string BackupHead { get; set; }

    /// <summary>
    /// The model used when an underwear item is equipped.
    /// </summary>
    /// <remarks>
    /// The modela column in 2DA.
    /// </remarks>
    public string UnderwearModel { get; set; }
    
    /// <summary>
    /// The texture to be applied to the underwear model.
    /// </summary>
    /// <remarks>
    /// The texa column in 2DA.
    /// </remarks>
    public string UnderwearTexture { get; set; }
    
    /// <summary>
    /// The texture to apply to the underwear model when the wearer skews heavily to the darkside.
    /// </summary>
    /// <remarks>
    /// The texaevil column in 2DA.
    /// </remarks>
    public string UnderwearTextureEvil { get; set; }

    /// <summary>
    /// The model used when a clothing item is equipped.
    /// </summary>
    /// <remarks>
    /// The modelb column in 2DA.
    /// </remarks>
    public string ClothingModel { get; set; }
    
    /// <summary>
    /// The texture to apply to the clothing model.
    /// </summary>
    /// <remarks>
    /// The texb column in 2DA.
    /// </remarks>
    public string ClothingTexture { get; set; }

    /// <summary>
    /// The model used when an armor (class 4) item is equipped.
    /// </summary>
    /// <remarks>
    /// The modelc column in 2DA.
    /// </remarks>
    public string ArmorClass4Model { get; set; }
    
    /// <summary>
    /// The texture to apply to the armor (class 4) model.
    /// </summary>
    /// <remarks>
    /// The texc column in 2DA.
    /// </remarks>
    public string ArmorClass4Texture { get; set; }

    /// <summary>
    /// The model used when an armor (class 5) item is equipped.
    /// </summary>
    /// <remarks>
    /// The modeld column in 2DA.
    /// </remarks>
    public string ArmorClass5Model { get; set; }
    
    /// <summary>
    /// The texture to apply to the armor (class 5) model.
    /// </summary>
    /// <remarks>
    /// The texd column in 2DA.
    /// </remarks>
    public string ArmorClass5Texture { get; set; }

    /// <summary>
    /// The model used when an armor (class 6) item is equipped.
    /// </summary>
    /// <remarks>
    /// The modele column in 2DA.
    /// </remarks>
    public string ArmorClass6Model { get; set; }
    
    /// <summary>
    /// The texture to apply to the armor (class 6) model.
    /// </summary>
    /// <remarks>
    /// The texe column in 2DA.
    /// </remarks>
    public string ArmorClass6Texture { get; set; }

    /// <summary>
    /// The model used when an armor (class 7) item is equipped.
    /// </summary>
    /// <remarks>
    /// The modelf column in 2DA.
    /// </remarks>
    public string ArmorClass7Model { get; set; }
    
    /// <summary>
    /// The texture to apply to the armor (class 7) model.
    /// </summary>
    /// <remarks>
    /// The texf column in 2DA.
    /// </remarks>
    public string ArmorClass7Texture { get; set; }

    /// <summary>
    /// The model used when an armor (class 8) item is equipped.
    /// </summary>
    /// <remarks>
    /// The modelg column in 2DA.
    /// </remarks>
    public string ArmorClass8Model { get; set; }
    
    /// <summary>
    /// The texture to apply to the armor (class 8) model.
    /// </summary>
    /// <remarks>
    /// The texg column in 2DA.
    /// </remarks>
    public string ArmorClass8Texture { get; set; }
    
    /// <summary>
    /// The model used when an armor (class 9) item is equipped.
    /// </summary>
    /// <remarks>
    /// The modelh column in 2DA.
    /// </remarks>
    public string ArmorClass9Model { get; set; }
    
    /// <summary>
    /// The texture to apply to the armor (class 9) model.
    /// </summary>
    /// <remarks>
    /// The texh column in 2DA.
    /// </remarks>
    public string ArmorClass9Texture { get; set; }

    /// <summary>
    /// The model used when a robes item is equipped.
    /// </summary>
    /// <remarks>
    /// The modeli column in 2DA.
    /// </remarks>
    public string RobesModel { get; set; }
    
    /// <summary>
    /// The texture to apply to the robes model.
    /// </summary>
    /// <remarks>
    /// The texi column in 2DA.
    /// </remarks>
    public string RobesTexture { get; set; }

    /// <summary>
    /// The model used when a starforge robes item is equipped.
    /// </summary>
    /// <remarks>
    /// The modelj column in 2DA.
    /// </remarks>
    public string StarForgeRobesModel { get; set; }

    /// <summary>
    /// The texture to apply to the starforge robes model.
    /// </summary>
    /// <remarks>
    /// The texj column in 2DA.
    /// </remarks>
    public string StarForgeRobesTexture { get; set; }

    /// <summary>
    /// The enviroment map texture to apply onto the creature model. Unknown if this will override the cubemap
    /// specified in the texture TXI file.
    /// </summary>
    /// <remarks>
    /// The envmap column in 2DA.
    /// </remarks>
    public string EnviromentMap { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The bloodclr column in 2DA.
    /// </remarks>
    public BloodColour BloodColour { get; set; }

    /// <summary>
    /// The scale factor for equipped weapon models.
    /// </summary>
    /// <remarks>
    /// The weaponscale column in 2DA.
    /// </remarks>
    public float WeaponScale { get; set; }

    // MoveRate

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The driveaccl column in 2DA.
    /// </remarks>
    public float DriveAcceleration { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The drivemaxspeed column in 2DA.
    /// </remarks>
    public float DriveMaxSpeed { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The hitradius column in 2DA.
    /// </remarks>
    public float HitRadius { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The abortonparry column in 2DA.
    /// </remarks>
    public bool AbortOnParry { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The haslegs and hasarms column in 2DA.
    /// </remarks>
    public bool IsTurret { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The footstepvolume column in 2DA. All vanilla game rows have this value set to 1.
    /// </remarks>
    public float FootStepVolume { get; set; }

    /// <summary>
    /// Index into the creaturesize.2da file.
    /// </summary>
    /// <remarks>
    /// The sizecategory column in 2DA.
    /// </remarks>
    public int CreatureSize { get; set; }

    /// <summary>
    /// Index into the ranges.2da file.
    /// </summary>
    /// <remarks>
    /// The perceptiondist column in 2DA.
    /// </remarks>
    public int PerceptionDistance { get; set; }

    /// <summary>
    /// Index into the footstepsounds.2da file.
    /// </summary>
    /// <remarks>
    /// The footsteptype column in 2DA.
    /// </remarks>
    public int FootStepType { get; set; }

    /// <summary>
    /// Index into the appearancesndset.2da file.
    /// </summary>
    /// <remarks>
    /// The soundapptype column in 2DA.
    /// </remarks>
    public int SoundAppType { get; set; }

    /// <summary>
    /// If set to true, then the head of the creature will turn to face whatever has its focus.
    /// </summary>
    /// <remarks>
    /// The headtrack column in 2DA.
    /// </remarks>
    public bool HeadTracking { get; set; }

    /// <summary>
    /// The degrees horizontally in which the creature's head can turn.
    /// </summary>
    /// <remarks>
    /// The head_arc_h column in 2DA.
    /// </remarks>
    public float MaxHorizontalHeadArc { get; set; }

    /// <summary>
    /// The degrees vertically in which the creature's head can turn.
    /// </summary>
    /// <remarks>
    /// The head_arc_v column in 2DA.
    /// </remarks>
    public float MaxVerticalHeadArc { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The hitdist column in 2DA.
    /// </remarks>
    public float HitDistance { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The prefatckdist column in 2DA.
    /// </remarks>
    public float PreferredAttackDistance { get; set; }

    /// <summary>
    /// If true, the angle of the creature adjusts to the slope of the walkmesh (see: T3-M4).
    /// </summary>
    /// <remarks>
    /// The groundtilt column in 2DA.
    /// </remarks>
    public bool GroundTilt { get; set; }

    /// <summary>
    /// The VFX filter to use when the PC enters freelook mode.
    /// </summary>
    /// <remarks>
    /// The freelookeffect column in 2DA. This is an index into the videoeffects.2da file.
    /// </remarks>
    public int FreeLookEffect { get; set; }

    /// <summary>
    /// The height offset in metres of the camera when controller by the player. A negative number represents
    /// a distance higher from the ground.
    /// </summary>
    /// <remarks>
    /// The cameraheightoffset column in 2DA.
    /// </remarks>
    public float? CameraHeightOffset { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The deathvfx column in 2DA. This is an index into the visualeffects.2da file.
    /// </remarks>
    public int DeathVFX { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The deathvfxnode column in 2DA. This is a reference to the name of a node in the VFX model.
    /// </remarks>
    public string DeathVFXNode { get; set; }
    
    /// <summary>
    /// If true, the creature will not play an injured animation when its health is low (see: Darth Malak).
    /// </summary>
    /// <remarks>
    /// The disableinjuredanim column in 2DA.
    /// </remarks>
    public bool DisableInjuredAnimation { get; set; }
}

public enum AppearanceModelType
{
    /// <summary>
    /// The creature renders both left and right weapons and the if the creature has a head it is assumed
    /// to be part of the main model.
    /// </summary>
    FullBody,

    /// <summary>
    /// The creature renders both left and right weapons and the head is rendered in a separate model to the
    /// main model.
    /// </summary>
    PartBody,

    RightWeaponOnly,

    NoWeapons,
}

public enum BloodColour
{
    Red,
    Green,
    White,
    Yellow,
    None,
    S,
}