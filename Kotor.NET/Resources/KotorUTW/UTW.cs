using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTW;

public class UTW
{
    public GFF Source { get; }

    public UTW()
    {
        Source = new();
    }
    public UTW(GFF source)
    {
        Source = source;
    }
    public static UTW FromFile(string filepath)
    {
        return new(GFF.FromFile(filepath));
    }
    public static UTW FromBytes(byte[] bytes)
    {
        return new(GFF.FromBytes(bytes));
    }
    public static UTW FromStream(Stream stream)
    {
        return new(GFF.FromStream(stream));
    }

    /// <summary>
    /// The appearance of the waypoint as displayed in the toolset.
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Appearance</c> field in the UTW. This is an index into the waypoint.2da file.
    /// </remarks>
    public byte AppearanceID
    {
        get => Source.Root.GetUInt8("Appearance") ?? 0;
        set => Source.Root.SetUInt8("Appearance", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>TemplateResRef</c> field in the UTW.
    /// </remarks>
    public ResRef ResRef
    {
        get => Source.Root.GetResRef("TemplateResRef") ?? "";
        set => Source.Root.SetResRef("TemplateResRef", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Tag</c> field in the UTW.
    /// </remarks>
    public string Tag
    {
        get => Source.Root.GetString("Tag") ?? "";
        set => Source.Root.SetString("Tag", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>LocalizedName</c> field in the UTW.
    /// </remarks>
    public LocalisedString Name
    {
        get => Source.Root.GetLocalisedString("LocalizedName") ?? new();
        set => Source.Root.SetLocalisedString("LocalizedName", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>HasMapNote</c> field in the UTW.
    /// </remarks>
    public bool HasMapNote
    {
        get => Source.Root.GetUInt8("HasMapNote") != 0;
        set => Source.Root.SetUInt8("HasMapNote", Convert.ToByte(value));
    }

    /// <remarks>
    /// This is the value stored in the <c>MapNote</c> field in the UTW.
    /// </remarks>
    public LocalisedString MapNote
    {
        get => Source.Root.GetLocalisedString("MapNote") ?? new();
        set => Source.Root.SetLocalisedString("MapNote", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>MapNoteEnabled</c> field in the UTW.
    /// </remarks>
    public bool MapNoteEnabled
    {
        get => Source.Root.GetUInt8("MapNoteEnabled") != 0;
        set => Source.Root.SetUInt8("MapNoteEnabled", Convert.ToByte(value));
    }

    /// <remarks>
    /// This is the value stored in the <c>PaletteID</c> field in the UTW.
    /// </remarks>
    public byte PaletteID
    {
        get => Source.Root.GetUInt8("PaletteID") ?? 0;
        set => Source.Root.SetUInt8("PaletteID", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Comment</c> field in the UTW.
    /// </remarks>
    public string Comment
    {
        get => Source.Root.GetString("Comment") ?? "";
        set => Source.Root.SetString("Comment", value);
    }
}
