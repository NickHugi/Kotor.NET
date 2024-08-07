using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTS;
public class UTS
{
    public GFF Source { get; }

    public UTS()
    {
        Source = new();
    }
    public UTS(GFF source)
    {
        Source = source;
    }
    public static UTS FromFile(string filepath)
    {
        return new(GFF.FromFile(filepath));
    }
    public static UTS FromBytes(byte[] bytes)
    {
        return new(GFF.FromBytes(bytes));
    }
    public static UTS FromStream(Stream stream)
    {
        return new(GFF.FromStream(stream));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Tag</c> field in the UTS.
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
    /// This is the value stored in the <c>LocName</c> field in the UTS.
    /// </remarks>
    public LocalisedString Name
    {
        get => Source.Root.GetLocalisedString("LocName") ?? new();
        set => Source.Root.SetLocalisedString("LocName", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>TemplateResRef</c> field in the UTS.
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
    /// This is the value stored in the <c>Active</c> field in the UTS.
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
    /// This is the value stored in the <c>Continuous</c> field in the UTS.
    /// </remarks>
    public bool Continuous
    {
        get => Source.Root.GetUInt8("Continuous") != 0;
        set => Source.Root.SetUInt8("Continuous", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Looping</c> field in the UTS.
    /// </remarks>
    public bool Looping
    {
        get => Source.Root.GetUInt8("Looping") != 0;
        set => Source.Root.SetUInt8("Looping", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Positional</c> field in the UTS.
    /// </remarks>
    public bool Positional
    {
        get => Source.Root.GetUInt8("Positional") != 0;
        set => Source.Root.SetUInt8("Positional", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>RandomPosition</c> field in the UTS.
    /// </remarks>
    public bool RandomPosition
    {
        get => Source.Root.GetUInt8("RandomPosition") != 0;
        set => Source.Root.SetUInt8("RandomPosition", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Random</c> field in the UTS.
    /// </remarks>
    public bool Random
    {
        get => Source.Root.GetUInt8("Random") != 0;
        set => Source.Root.SetUInt8("Random", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Elevation</c> field in the UTS.
    /// </remarks>
    public float Elevation
    {
        get => Source.Root.GetSingle("Elevation") ?? 0.0f;
        set => Source.Root.SetSingle("Elevation", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>MaxDistance</c> field in the UTS.
    /// </remarks>
    public float MaxDistance
    {
        get => Source.Root.GetSingle("MaxDistance") ?? 0.0f;
        set => Source.Root.SetSingle("MaxDistance", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>MinDistance</c> field in the UTS.
    /// </remarks>
    public float MinDistance
    {
        get => Source.Root.GetSingle("MinDistance") ?? 0.0f;
        set => Source.Root.SetSingle("MinDistance", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>RandomRangeX</c> field in the UTS.
    /// </remarks>
    public float RandomRangeX
    {
        get => Source.Root.GetSingle("RandomRangeX") ?? 0.0f;
        set => Source.Root.SetSingle("RandomRangeX", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>RandomRangeY</c> field in the UTS.
    /// </remarks>
    public float RandomRangeY
    {
        get => Source.Root.GetSingle("RandomRangeY") ?? 0.0f;
        set => Source.Root.SetSingle("RandomRangeY", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Interval</c> field in the UTS.
    /// </remarks>
    public uint Interval
    {
        get => Source.Root.GetUInt32("Interval") ?? 0;
        set => Source.Root.SetUInt32("Interval", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>IntervalVrtn</c> field in the UTS.
    /// </remarks>
    public uint IntervalVariation
    {
        get => Source.Root.GetUInt32("IntervalVrtn") ?? 0;
        set => Source.Root.SetUInt32("IntervalVrtn", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>PitchVariation</c> field in the UTS.
    /// </remarks>
    public float PitchVariation
    {
        get => Source.Root.GetSingle("PitchVariation") ?? 0.0f;
        set => Source.Root.SetSingle("PitchVariation", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Priority</c> field in the UTS.
    /// </remarks>
    public byte Priority
    {
        get => Source.Root.GetUInt8("Priority") ?? 0;
        set => Source.Root.SetUInt8("Priority", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Volume</c> field in the UTS.
    /// </remarks>
    public byte Volume
    {
        get => Source.Root.GetUInt8("Volume") ?? 0;
        set => Source.Root.SetUInt8("Volume", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>VolumeVrtn</c> field in the UTS.
    /// </remarks>
    public byte VolumeVariation
    {
        get => Source.Root.GetUInt8("VolumeVrtn") ?? 0;
        set => Source.Root.SetUInt8("VolumeVrtn", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>PaletteID</c> field in the UTS.
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
    /// This is the value stored in the <c>Comment</c> field in the UTS.
    /// </remarks>
    public string Comment
    {
        get => Source.Root.GetString("Comment") ?? "";
        set => Source.Root.SetString("Comment", value);
    }

    public UTSSoundCollection Sounds => new(Source);
}
