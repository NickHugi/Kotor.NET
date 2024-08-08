using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorIFO;

public class IFO
{
    public GFF Source { get; }

    public IFO()
    {
        Source = new();
    }
    public IFO(GFF source)
    {
        Source = source;
    }
    public static IFO FromFile(string filepath)
    {
        return new(GFF.FromFile(filepath));
    }
    public static IFO FromBytes(byte[] bytes)
    {
        return new(GFF.FromBytes(bytes));
    }
    public static IFO FromStream(Stream stream)
    {
        return new(GFF.FromStream(stream));
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_VO_ID</c> field in the IFO.
    /// </remarks>
    public string VoiceOverID
    {
        get => Source.Root.GetString("Mod_VO_ID") ?? "";
        set => Source.Root.SetString("Mod_VO_ID", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_Name</c> field in the IFO.
    /// </remarks>
    public LocalisedString Name
    {
        get => Source.Root.GetLocalisedString("Mod_Name") ?? new();
        set => Source.Root.SetLocalisedString("Mod_Name", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_Tag</c> field in the IFO.
    /// </remarks>
    public string Tag
    {
        get => Source.Root.GetString("Mod_Tag") ?? "";
        set => Source.Root.SetString("Mod_Tag", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_Entry_Area</c> field in the IFO.
    /// </remarks>
    public ResRef EntryArea
    {
        get => Source.Root.GetResRef("Mod_Entry_Area") ?? "";
        set => Source.Root.SetResRef("Mod_Entry_Area", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_Entry_X</c> field in the IFO.
    /// </remarks>
    public float ModEntryX
    {
        get => Source.Root.GetSingle("Mod_Entry_X") ?? 0.0f;
        set => Source.Root.SetSingle("Mod_Entry_X", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_Entry_Y</c> field in the IFO.
    /// </remarks>
    public float ModEntryY
    {
        get => Source.Root.GetSingle("Mod_Entry_Y") ?? 0.0f;
        set => Source.Root.SetSingle("Mod_Entry_Y", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_Entry_Z</c> field in the IFO.
    /// </remarks>
    public float ModEntryZ
    {
        get => Source.Root.GetSingle("Mod_Entry_Z") ?? 0.0f;
        set => Source.Root.SetSingle("Mod_Entry_Z", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_Entry_Dir_X</c> field in the IFO.
    /// </remarks>
    public float ModEntryDirectionX
    {
        get => Source.Root.GetSingle("Mod_Entry_Dir_X") ?? 0.0f;
        set => Source.Root.SetSingle("Mod_Entry_Dir_X", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_Entry_Dir_Y</c> field in the IFO.
    /// </remarks>
    public float ModEntryDirectionY
    {
        get => Source.Root.GetSingle("Mod_Entry_Dir_Y") ?? 0.0f;
        set => Source.Root.SetSingle("Mod_Entry_Dir_Y", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_XPScale</c> field in the IFO.
    /// </remarks>
    public byte XPScale
    {
        get => Source.Root.GetUInt8("Mod_XPScale") ?? 0;
        set => Source.Root.SetUInt8("Mod_XPScale", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_OnHeartbeat</c> field in the IFO.
    /// </remarks>
    public ResRef OnHeartbeat
    {
        get => Source.Root.GetResRef("Mod_OnHeartbeat") ?? "";
        set => Source.Root.SetResRef("Mod_OnHeartbeat", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_OnModLoad</c> field in the IFO.
    /// </remarks>
    public ResRef OnModLoad
    {
        get => Source.Root.GetResRef("Mod_OnModLoad") ?? "";
        set => Source.Root.SetResRef("Mod_OnModLoad", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_OnModStart</c> field in the IFO.
    /// </remarks>
    public ResRef OnModStart
    {
        get => Source.Root.GetResRef("Mod_OnModStart") ?? "";
        set => Source.Root.SetResRef("Mod_OnModStart", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_OnClientEntr</c> field in the IFO.
    /// </remarks>
    public ResRef OnClientEnter
    {
        get => Source.Root.GetResRef("Mod_OnClientEntr") ?? "";
        set => Source.Root.SetResRef("Mod_OnClientEntr", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_OnClientLeav</c> field in the IFO.
    /// </remarks>
    public ResRef OnClientLeave
    {
        get => Source.Root.GetResRef("Mod_OnClientLeav") ?? "";
        set => Source.Root.SetResRef("Mod_OnClientLeav", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_OnActvtItem</c> field in the IFO.
    /// </remarks>
    public ResRef OnActivateItem
    {
        get => Source.Root.GetResRef("Mod_OnActvtItem") ?? "";
        set => Source.Root.SetResRef("Mod_OnActvtItem", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_OnAcquirItem</c> field in the IFO.
    /// </remarks>
    public ResRef OnAcquireItem
    {
        get => Source.Root.GetResRef("Mod_OnAcquirItem") ?? "";
        set => Source.Root.SetResRef("Mod_OnAcquirItem", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_OnUsrDefined</c> field in the IFO.
    /// </remarks>
    public ResRef OnUserDefined
    {
        get => Source.Root.GetResRef("Mod_OnUsrDefined") ?? "";
        set => Source.Root.SetResRef("Mod_OnUsrDefined", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_OnUnAqreItem</c> field in the IFO.
    /// </remarks>
    public ResRef OnUnAcquireItem
    {
        get => Source.Root.GetResRef("Mod_OnUnAqreItem") ?? "";
        set => Source.Root.SetResRef("Mod_OnUnAqreItem", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_OnPlrDeath</c> field in the IFO.
    /// </remarks>
    public ResRef OnPlayerDeath
    {
        get => Source.Root.GetResRef("Mod_OnPlrDeath") ?? "";
        set => Source.Root.SetResRef("Mod_OnPlrDeath", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_OnPlrDying</c> field in the IFO.
    /// </remarks>
    public ResRef OnPlayerDying
    {
        get => Source.Root.GetResRef("Mod_OnPlrDying") ?? "";
        set => Source.Root.SetResRef("Mod_OnPlrDying", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_OnPlrLvlUp</c> field in the IFO.
    /// </remarks>
    public ResRef OnPlayerLevelUp
    {
        get => Source.Root.GetResRef("Mod_OnPlrLvlUp") ?? "";
        set => Source.Root.SetResRef("Mod_OnPlrLvlUp", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Mod_OnSpawnBtnDn</c> field in the IFO.
    /// </remarks>
    public ResRef OnSpawnButtonDown
    {
        get => Source.Root.GetResRef("Mod_OnSpawnBtnDn") ?? "";
        set => Source.Root.SetResRef("Mod_OnSpawnBtnDn", value);
    }

    public IFOAreaCollection ModAreaList => new(Source);
}
