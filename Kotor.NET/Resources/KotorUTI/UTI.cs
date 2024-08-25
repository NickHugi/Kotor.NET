using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTI;

public class UTI
{
    public GFF Source { get; }

    public UTI()
    {
        Source = new();
    }
    public UTI(GFF source)
    {
        Source = source;
    }
    public static UTI FromFile(string filepath)
    {
        return new(GFF.FromFile(filepath));
    }
    public static UTI FromBytes(byte[] bytes)
    {
        return new(GFF.FromBytes(bytes));
    }
    public static UTI FromStream(Stream stream)
    {
        return new(GFF.FromStream(stream));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>AddCost</c> field in the UTI.
    /// </remarks>
    public uint AddCost
    {
        get => Source.Root.GetUInt32("AddCost") ?? 0;
        set => Source.Root.SetUInt32("AddCost", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>BaseItem</c> field in the UTI.
    /// </remarks>
    public int BaseItem
    {
        get => Source.Root.GetInt32("BaseItem") ?? 0;
        set => Source.Root.SetInt32("BaseItem", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>BodyVariation</c> field in the UTI.
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
    /// This is the value stored in the <c>Charges</c> field in the UTI.
    /// </remarks>
    public byte Charges
    {
        get => Source.Root.GetUInt8("Charges") ?? 0;
        set => Source.Root.SetUInt8("Charges", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Comment</c> field in the UTI.
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
    /// This is the value stored in the <c>Cost</c> field in the UTI.
    /// </remarks>
    public uint Cost
    {
        get => Source.Root.GetUInt32("Cost") ?? 0;
        set => Source.Root.SetUInt32("Cost", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DescIdentified</c> field in the UTI.
    /// </remarks>
    public LocalisedString DescriptionIdentified
    {
        get => Source.Root.GetLocalisedString("DescIdentified") ?? new();
        set => Source.Root.SetLocalisedString("DescIdentified", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Description</c> field in the UTI.
    /// </remarks>
    public LocalisedString DescriptionUnidentified
    {
        get => Source.Root.GetLocalisedString("Description") ?? new();
        set => Source.Root.SetLocalisedString("Description", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>LocalizedName</c> field in the UTI.
    /// </remarks>
    public LocalisedString LocalizedName
    {
        get => Source.Root.GetLocalisedString("LocalizedName") ?? new();
        set => Source.Root.SetLocalisedString("LocalizedName", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ModelVariation</c> field in the UTI.
    /// </remarks>
    public byte ModelVariation
    {
        get => Source.Root.GetUInt8("ModelVariation") ?? 0;
        set => Source.Root.SetUInt8("ModelVariation", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>PaletteID</c> field in the UTI.
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
    /// This is the value stored in the <c>Plot</c> field in the UTI.
    /// </remarks>
    public bool Plot
    {
        get => (Source.Root.GetUInt8("Plot") ?? 0) != 0;
        set => Source.Root.SetUInt8("Plot", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>StackSize</c> field in the UTI.
    /// </remarks>
    public ushort StackSize
    {
        get => Source.Root.GetUInt16("StackSize") ?? 0;
        set => Source.Root.SetUInt16("StackSize", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Tag</c> field in the UTI.
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
    /// This is the value stored in the <c>TemplateResRef</c> field in the UTI.
    /// </remarks>
    public ResRef TemplateResRef
    {
        get => Source.Root.GetResRef("TemplateResRef") ?? "";
        set => Source.Root.SetResRef("TemplateResRef", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>TextureVar</c> field in the UTI.
    /// </remarks>
    public byte TextureVar
    {
        get => Source.Root.GetUInt8("TextureVar") ?? 0;
        set => Source.Root.SetUInt8("TextureVar", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>UpgradeLevel</c> field in the UTI.
    /// </remarks>
    public byte UpgradeLevel
    {
        get => Source.Root.GetUInt8("UpgradeLevel") ?? 0;
        set => Source.Root.SetUInt8("UpgradeLevel", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Identified</c> field in the UTI.
    /// </remarks>
    public bool Identified
    {
        get => (Source.Root.GetUInt8("Identified") ?? 0) != 0;
        set => Source.Root.SetUInt8("Identified", Convert.ToByte(value));
    }

    public UTIPropertyCollection Properties => new(Source);
}
