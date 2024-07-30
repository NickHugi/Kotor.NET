using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryGFF;
using Kotor.NET.Resources.KotorGFF;
using static Kotor.NET.Resources.KotorUTC.UTCClassCollection;

namespace Kotor.NET.Resources.KotorUTC;

public class UTC
{
    public GFF Source { get; }

    public UTC()
    {
        Source = new();
    }
    public UTC(GFF source)
    {
        Source = source;
    }
    public static UTC FromFile(string filepath)
    {
        return new(GFF.FromFile(filepath));
    }
    public static UTC FromBytes(byte[] bytes)
    {
        return new(GFF.FromBytes(bytes));
    }
    public static UTC FromStream(Stream stream)
    {
        return new(GFF.FromStream(stream));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Appearance_Type</c> field in the UTC and is an index
    /// into the <c>appearance.2da</c> file.
    /// </remarks>
    public ushort AppearanceID
    {
        get => Source.Root.GetUInt16("Appearance_Type") ?? 0;
        set => Source.Root.SetUInt16("Appearance_Type", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>BlindSpot</c> field in the UTC.
    /// </remarks>
    public float BlindSpot
    {
        get => Source.Root.GetSingle("BlindSpot") ?? 0;
        set => Source.Root.SetSingle("BlindSpot", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>BodyVariation</c> field in the UTC.
    /// </remarks>
    public byte BodyVariation
    {
        get => Source.Root.GetUInt8("BodyVariation") ?? 0;
        set => Source.Root.SetUInt8("BodyVariation", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Cha</c> field in the UTC.
    /// </remarks>
    public byte Charisma
    {
        get => Source.Root.GetUInt8("Cha") ?? 0;
        set => Source.Root.SetUInt8("Cha", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ChallengeRating</c> field in the UTC.
    /// </remarks>
    public float ChallengeRating
    {
        get => Source.Root.GetSingle("ChallengeRating") ?? 0;
        set => Source.Root.SetSingle("ChallengeRating", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Comment</c> field in the UTC.
    /// </remarks>
    public string Comment
    {
        get => Source.Root.GetString("Comment") ?? "";
        set => Source.Root.SetString("Comment", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Con</c> field in the UTC.
    /// </remarks>
    public byte Constitution
    {
        get => Source.Root.GetUInt8("Con") ?? 0;
        set => Source.Root.SetUInt8("Con", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Conversation</c> field in the UTC.
    /// </remarks>
    public ResRef Conversation
    {
        get => Source.Root.GetResRef("Conversation") ?? "";
        set => Source.Root.SetResRef("Conversation", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>CurrentForce</c> field in the UTC.
    /// </remarks>
    public short CurrentForce
    {
        get => Source.Root.GetInt16("CurrentForce") ?? 0;
        set => Source.Root.SetInt16("CurrentForce", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>CurrentHitPoints</c> field in the UTC.
    /// </remarks>
    public short CurrentHitPoints
    {
        get => Source.Root.GetInt16("CurrentHitPoints") ?? 0;
        set => Source.Root.SetInt16("CurrentHitPoints", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Dex</c> field in the UTC.
    /// </remarks>
    public byte Dexterity
    {
        get => Source.Root.GetUInt8("Dex") ?? 0;
        set => Source.Root.SetUInt8("Dex", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Disarmable</c> field in the UTC.
    /// </remarks>
    public bool Disarmable
    {
        get => Source.Root.GetUInt8("Disarmable") != 0;
        set => Source.Root.SetUInt8("Disarmable", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>FactionID</c> field in the UTC and is an index into
    /// the <c>repute.2da</c> file.
    /// </remarks>
    public ushort FactionID
    {
        get => Source.Root.GetUInt16("FactionID") ?? 0;
        set => Source.Root.SetUInt16("FactionID", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>FirstName</c> field in the UTC.
    /// </remarks>
    public LocalisedString FirstName
    {
        get => Source.Root.GetLocalisedString("FirstName") ?? new();
        set => Source.Root.SetLocalisedString("FirstName", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ForcePoints</c> field in the UTC.
    /// </remarks>
    public short ForcePoints
    {
        get => Source.Root.GetInt16("ForcePoints") ?? 0;
        set => Source.Root.SetInt16("ForcePoints", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>fortbonus</c> field in the UTC.
    /// </remarks>
    public short FortitudeBonus
    {
        get => Source.Root.GetInt16("fortbonus") ?? 0;
        set => Source.Root.SetInt16("fortbonus", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Gender</c> field in the UTC.
    /// </remarks>
    public byte Gender
    {
        get => Source.Root.GetUInt8("Gender") ?? 0;
        set => Source.Root.SetUInt8("Gender", value);
    }

    /// <summary>
    /// Where on the spectrum of good and evil the creature lays on. A value of 0, 50 and 100
    /// represent darkside, neutral and lightside respectively.
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>GoodEvil</c> field in the UTC.
    /// </remarks>
    public byte Alignment
    {
        get => Source.Root.GetUInt8("GoodEvil") ?? 0;
        set => Source.Root.SetUInt8("GoodEvil", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>HitPoints</c> field in the UTC.
    /// </remarks>
    public short HitPoints
    {
        get => Source.Root.GetInt16("HitPoints") ?? 0;
        set => Source.Root.SetInt16("HitPoints", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Int</c> field in the UTC.
    /// </remarks>
    public byte Intelligence
    {
        get => Source.Root.GetUInt8("Int") ?? 0;
        set => Source.Root.SetUInt8("Int", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Hologram</c> field in the UTC. Only supported by KotOR 2.
    /// </remarks>
    public bool IsHologram
    {
        get => Source.Root.GetUInt8("Hologram") != 0;
        set => Source.Root.SetUInt8("Hologram", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>IgnoreCrePath</c> field in the UTC.
    /// </remarks>
    public bool IgnoreCrePath
    {
        get => Source.Root.GetUInt8("IgnoreCrePath") != 0;
        set => Source.Root.SetUInt8("IgnoreCrePath", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Interruptable</c> field in the UTC.
    /// </remarks>
    public bool Interruptable
    {
        get => Source.Root.GetUInt8("Interruptable") != 0;
        set => Source.Root.SetUInt8("Interruptable", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>IsPC</c> field in the UTC.
    /// </remarks>
    public bool IsPC
    {
        get => Source.Root.GetUInt8("IsPC") != 0;
        set => Source.Root.SetUInt8("IsPC", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>LastName</c> field in the UTC.
    /// </remarks>
    public LocalisedString LastName
    {
        get => Source.Root.GetLocalisedString("LastName") ?? new();
        set => Source.Root.SetLocalisedString("LastName", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>MaxHitPoints</c> field in the UTC.
    /// </remarks>
    public short MaxHitPoints
    {
        get => Source.Root.GetInt16("MaxHitPoints") ?? 0;
        set => Source.Root.SetInt16("MaxHitPoints", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Min1HP</c> field in the UTC.
    /// </remarks>
    public bool Min1HP
    {
        get => Source.Root.GetUInt8("Min1HP") != 0;
        set => Source.Root.SetUInt8("Min1HP", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>MultiplierSet</c> field in the UTC.
    /// </remarks>
    public byte MultiplierSet
    {
        get => Source.Root.GetUInt8("MultiplierSet") ?? 0;
        set => Source.Root.SetUInt8("MultiplierSet", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>NaturalAC</c> field in the UTC.
    /// </remarks>
    public byte NaturalAC
    {
        get => Source.Root.GetUInt8("NaturalAC") ?? 0;
        set => Source.Root.SetUInt8("NaturalAC", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>NoPermDeath</c> field in the UTC.
    /// </remarks>
    public bool NoPermDeath
    {
        get => Source.Root.GetUInt8("NoPermDeath") != 0;
        set => Source.Root.SetUInt8("NoPermDeath", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>NotReorienting</c> field in the UTC.
    /// </remarks>
    public bool NotReorienting
    {
        get => Source.Root.GetUInt8("NotReorienting") != 0;
        set => Source.Root.SetUInt8("NotReorienting", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>PaletteID</c> field in the UTC.
    /// </remarks>
    public byte PaletteID
    {
        get => Source.Root.GetUInt8("PaletteID") ?? 0;
        set => Source.Root.SetUInt8("PaletteID", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>PartyInteract</c> field in the UTC.
    /// </remarks>
    public bool PartyInteract
    {
        get => Source.Root.GetUInt8("PartyInteract") != 0;
        set => Source.Root.SetUInt8("PartyInteract", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>PerceptionRange</c> field in the UTC and is an index into
    /// the <c>ranges.2da</c> file.
    /// </remarks>
    public byte PerceptionRangeID
    {
        get => Source.Root.GetUInt8("PerceptionRange") ?? 0;
        set => Source.Root.SetUInt8("PerceptionRange", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Plot</c> field in the UTC.
    /// </remarks>
    public bool Plot
    {
        get => Source.Root.GetUInt8("Plot") != 0;
        set => Source.Root.SetUInt8("Plot", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Race</c> field in the UTC and is an index into the
    /// <c>racialtypes.2da</c> file.
    /// </remarks>
    public byte RaceID
    {
        get => Source.Root.GetUInt8("Race") ?? 0;
        set => Source.Root.SetUInt8("Race", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>refbonus</c> field in the UTC.
    /// </remarks>
    public short ReflexBonus
    {
        get => Source.Root.GetInt16("refbonus") ?? 0;
        set => Source.Root.SetInt16("refbonus", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ScriptAttacked</c> field in the UTC.
    /// </remarks>
    public ResRef OnAttacked
    {
        get => Source.Root.GetResRef("ScriptAttacked") ?? "";
        set => Source.Root.SetResRef("ScriptAttacked", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ScriptDamaged</c> field in the UTC.
    /// </remarks>
    public ResRef OnDamaged
    {
        get => Source.Root.GetResRef("ScriptDamaged") ?? "";
        set => Source.Root.SetResRef("ScriptDamaged", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ScriptDeath</c> field in the UTC.
    /// </remarks>
    public ResRef OnDeath
    {
        get => Source.Root.GetResRef("ScriptDeath") ?? "";
        set => Source.Root.SetResRef("ScriptDeath", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ScriptDialogue</c> field in the UTC.
    /// </remarks>
    public ResRef OnDialogue
    {
        get => Source.Root.GetResRef("ScriptDialogue") ?? "";
        set => Source.Root.SetResRef("ScriptDialogue", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ScriptDisturbed</c> field in the UTC.
    /// </remarks>
    public ResRef OnDisturbed
    {
        get => Source.Root.GetResRef("ScriptDisturbed") ?? "";
        set => Source.Root.SetResRef("ScriptDisturbed", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ScriptEndDialogu</c> field in the UTC.
    /// </remarks>
    public ResRef OnEndDialogue
    {
        get => Source.Root.GetResRef("ScriptEndDialogu") ?? "";
        set => Source.Root.SetResRef("ScriptEndDialogu", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ScriptEndRound</c> field in the UTC.
    /// </remarks>
    public ResRef OnEndRound
    {
        get => Source.Root.GetResRef("ScriptEndRound") ?? "";
        set => Source.Root.SetResRef("ScriptEndRound", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ScriptHeartbeat</c> field in the UTC.
    /// </remarks>
    public ResRef OnHeartbeat
    {
        get => Source.Root.GetResRef("ScriptHeartbeat") ?? "";
        set => Source.Root.SetResRef("ScriptHeartbeat", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ScriptOnBlocked</c> field in the UTC.
    /// </remarks>
    public ResRef OnBlocked
    {
        get => Source.Root.GetResRef("ScriptOnBlocked") ?? "";
        set => Source.Root.SetResRef("ScriptOnBlocked", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ScriptOnNotice</c> field in the UTC.
    /// </remarks>
    public ResRef OnNoticed
    {
        get => Source.Root.GetResRef("ScriptOnNotice") ?? "";
        set => Source.Root.SetResRef("ScriptOnNotice", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ScriptSpawn</c> field in the UTC.
    /// </remarks>
    public ResRef OnSpawned
    {
        get => Source.Root.GetResRef("ScriptSpawn") ?? "";
        set => Source.Root.SetResRef("ScriptSpawn", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ScriptSpellAt</c> field in the UTC.
    /// </remarks>
    public ResRef OnSpellAt
    {
        get => Source.Root.GetResRef("ScriptSpellAt") ?? "";
        set => Source.Root.SetResRef("ScriptSpellAt", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ScriptUserDefine</c> field in the UTC.
    /// </remarks>
    public ResRef OnUserDefined
    {
        get => Source.Root.GetResRef("ScriptUserDefine") ?? "";
        set => Source.Root.SetResRef("ScriptUserDefine", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SoundSetFile</c> field in the UTC and is an index into
    /// the <c>soundset.2da</c> file.
    /// </remarks>
    public ushort SoundSetID
    {
        get => Source.Root.GetUInt16("SoundSetFile") ?? 0;
        set => Source.Root.SetUInt16("SoundSetFile", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Str</c> field in the UTC.
    /// </remarks>
    public byte Strength
    {
        get => Source.Root.GetUInt8("Str") ?? 0;
        set => Source.Root.SetUInt8("Str", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SubraceIndex</c> field in the UTC and is an index into
    /// the <c>subrace.2da</c> file.
    /// </remarks>
    public byte SubraceID
    {
        get => Source.Root.GetUInt8("SubraceIndex") ?? 0;
        set => Source.Root.SetUInt8("SubraceIndex", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Tag</c> field in the UTC.
    /// </remarks>
    public string Tag
    {
        get => Source.Root.GetString("Tag") ?? "";
        set => Source.Root.SetString("Tag", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>TemplateResRef</c> field in the UTC.
    /// </remarks>
    public ResRef ResourceResRef
    {
        get => Source.Root.GetResRef("TemplateResRef") ?? "";
        set => Source.Root.SetResRef("TemplateResRef", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>TextureVar</c> field in the UTC.
    /// </remarks>
    public byte TextureVariation
    {
        get => Source.Root.GetUInt8("TextureVar") ?? 0;
        set => Source.Root.SetUInt8("TextureVar", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>WalkRate</c> field in the UTC and is an index into
    /// the <c>creaturespeed.2da</c> file.
    /// </remarks>
    public int CreatureSpeedID
    {
        get => Source.Root.GetInt32("WalkRate") ?? 0;
        set => Source.Root.SetInt32("WalkRate", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>XYZ</c> field in the UTC.
    /// </remarks>
    public short WillBonus
    {
        get => Source.Root.GetInt16("willbonus") ?? 0;
        set => Source.Root.SetInt16("willbonus", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>WillNotRender</c> field in the UTC.
    /// </remarks>
    public bool WillNotRender
    {
        get => Source.Root.GetUInt8("WillNotRender") != 0;
        set => Source.Root.SetUInt8("WillNotRender", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Wis</c> field in the UTC.
    /// </remarks>
    public byte Wisdom
    {
        get => Source.Root.GetUInt8("Wis") ?? 0;
        set => Source.Root.SetUInt8("Wis", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SkillList/0/Rank</c> field in the UTC.
    /// </remarks>
    public byte ComputerUse
    {
        get => Source.Root.GetList("SkillList")?.ElementAt(0)?.GetUInt8("Rank") ?? 0;
        set => GetSkillList().ElementAt(0).SetUInt8("Rank", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SkillList/1/Rank</c> field in the UTC.
    /// </remarks>
    public byte Demolitions
    {
        get => Source.Root.GetList("SkillList")?.ElementAt(1)?.GetUInt8("Rank") ?? 0;
        set => GetSkillList().ElementAt(1).SetUInt8("Rank", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SkillList/2/Rank</c> field in the UTC.
    /// </remarks>
    public byte Stealth
    {
        get => Source.Root.GetList("SkillList")?.ElementAt(2)?.GetUInt8("Rank") ?? 0;
        set => GetSkillList().ElementAt(2).SetUInt8("Rank", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SkillList/3/Rank</c> field in the UTC.
    /// </remarks>
    public byte Awareness
    {
        get => Source.Root.GetList("SkillList")?.ElementAt(3)?.GetUInt8("Rank") ?? 0;
        set => GetSkillList().ElementAt(3).SetUInt8("Rank", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SkillList/4/Rank</c> field in the UTC.
    /// </remarks>
    public byte Persuade
    {
        get => Source.Root.GetList("SkillList")?.ElementAt(4)?.GetUInt8("Rank") ?? 0;
        set => GetSkillList().ElementAt(4).SetUInt8("Rank", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SkillList/5/Rank</c> field in the UTC.
    /// </remarks>
    public byte Repair
    {
        get => Source.Root.GetList("SkillList")?.ElementAt(5)?.GetUInt8("Rank") ?? 0;
        set => GetSkillList().ElementAt(5).SetUInt8("Rank", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SkillList/6/Rank</c> field in the UTC.
    /// </remarks>
    public byte Security
    {
        get => Source.Root.GetList("SkillList")?.ElementAt(6)?.GetUInt8("Rank") ?? 0;
        set => GetSkillList().ElementAt(6).SetUInt8("Rank", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SkillList/7/Rank</c> field in the UTC.
    /// </remarks>
    public byte TreatInjury
    {
        get => Source.Root.GetList("SkillList")?.ElementAt(7)?.GetUInt8("Rank") ?? 0;
        set => GetSkillList().ElementAt(7).SetUInt8("Rank", value);
    }

    public UTCClassCollection Classes => new(Source);
    public UTCFeatCollection Feats => new(Source);
    public UTCItemCollection Items => new(Source);
    public UTCEquipment Equipment => new(Source);

    private GFFList GetSkillList()
    {
        var skillList = Source.Root.GetList("SkillList") ?? Source.Root.SetList("SkillList");
        for (int i = 0; i < skillList.Count - skillList.Count; i++)
        {
            var skillStruct = skillList.Add();
            skillStruct.SetUInt8("Rank", 0);
        }
        return skillList;
    }
}
