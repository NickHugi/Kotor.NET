using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTE;

public class UTE
{
    public GFF Source { get; }

    public UTE()
    {
        Source = new();
    }
    public UTE(GFF source)
    {
        Source = source;
    }
    public static UTE FromFile(string filepath)
    {
        return new(GFF.FromFile(filepath));
    }
    public static UTE FromBytes(byte[] bytes)
    {
        return new(GFF.FromBytes(bytes));
    }
    public static UTE FromStream(Stream stream)
    {
        return new(GFF.FromStream(stream));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>LocalizedName</c> field in the UTE.
    /// </remarks>
    public LocalisedString Name
    {
        get => Source.Root.GetLocalisedString("LocalizedName") ?? new();
        set => Source.Root.SetLocalisedString("LocalizedName", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>TemplateResRef</c> field in the UTE.
    /// </remarks>
    public ResRef ResRef
    {
        get => Source.Root.GetResRef("TemplateResRef") ?? "";
        set => Source.Root.SetResRef("TemplateResRef", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Active</c> field in the UTE.
    /// </remarks>
    public bool Active
    {
        get => Source.Root.GetUInt8("Active") != 0;
        set => Source.Root.SetUInt8("Active", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Difficulty</c> field in the UTE.
    /// </remarks>
    public int Difficulty
    {
        get => Source.Root.GetInt32("Difficulty") ?? 0;
        set => Source.Root.SetInt32("Difficulty", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DifficultyIndex</c> field in the UTE.
    /// </remarks>
    public int DifficultyIndex
    {
        get => Source.Root.GetInt32("DifficultyIndex") ?? 0;
        set => Source.Root.SetInt32("DifficultyIndex", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Faction</c> field in the UTE. This is an index into the <c>repute.2da</c> file.
    /// </remarks>
    public uint FactionID
    {
        get => Source.Root.GetUInt32("Faction") ?? 0;
        set => Source.Root.SetUInt32("Faction", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>MaxCreatures</c> field in the UTE.
    /// </remarks>
    public int MaxCreatures
    {
        get => Source.Root.GetInt32("MaxCreatures") ?? 0;
        set => Source.Root.SetInt32("MaxCreatures", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>PlayerOnly</c> field in the UTE.
    /// </remarks>
    public bool PlayerOnly
    {
        get => Source.Root.GetUInt8("PlayerOnly") != 0;
        set => Source.Root.SetUInt8("PlayerOnly", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>RecCreatures</c> field in the UTE.
    /// </remarks>
    public int RecommendedCreatures
    {
        get => Source.Root.GetInt32("RecCreatures") ?? 0;
        set => Source.Root.SetInt32("RecCreatures", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Reset</c> field in the UTE.
    /// </remarks>
    public bool Reset
    {
        get => Source.Root.GetUInt8("Reset") != 0;
        set => Source.Root.SetUInt8("Reset", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ResetTime</c> field in the UTE.
    /// </remarks>
    public int ResetTime
    {
        get => Source.Root.GetInt32("ResetTime") ?? 0;
        set => Source.Root.SetInt32("ResetTime", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Respawns</c> field in the UTE.
    /// </remarks>
    public int Respawns
    {
        get => Source.Root.GetInt32("Respawns") ?? 0;
        set => Source.Root.SetInt32("Respawns", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SpawnOption</c> field in the UTE.
    /// </remarks>
    public int SpawnOption
    {
        get => Source.Root.GetInt32("SpawnOption") ?? 0;
        set => Source.Root.SetInt32("SpawnOption", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnEntered</c> field in the UTE.
    /// </remarks>
    public ResRef OnEntered
    {
        get => Source.Root.GetResRef("OnEntered") ?? "";
        set => Source.Root.SetResRef("OnEntered", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnExit</c> field in the UTE.
    /// </remarks>
    public ResRef OnExit
    {
        get => Source.Root.GetResRef("OnExit") ?? "";
        set => Source.Root.SetResRef("OnExit", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnExhausted</c> field in the UTE.
    /// </remarks>
    public ResRef OnExhausted
    {
        get => Source.Root.GetResRef("OnExhausted") ?? "";
        set => Source.Root.SetResRef("OnExhausted", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnHeartbeat</c> field in the UTE.
    /// </remarks>
    public ResRef OnHeartbeat
    {
        get => Source.Root.GetResRef("OnHeartbeat") ?? "";
        set => Source.Root.SetResRef("OnHeartbeat", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnUserDefined</c> field in the UTE.
    /// </remarks>
    public ResRef OnUserDefined
    {
        get => Source.Root.GetResRef("OnUserDefined") ?? "";
        set => Source.Root.SetResRef("OnUserDefined", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>PaletteID</c> field in the UTE.
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
    /// This is the value stored in the <c>Comment</c> field in the UTE.
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
    /// This is the value stored in the <c>Tag</c> field in the UTE.
    /// </remarks>
    public string Tag
    {
        get => Source.Root.GetString("Tag") ?? "";
        set => Source.Root.SetString("Tag", value);
    }

    public UTECreatureCollection Creatures => new(Source);
}
