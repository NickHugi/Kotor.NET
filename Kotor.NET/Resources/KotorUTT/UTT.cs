using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTT;

public class UTT
{
    public GFF Source { get; }

    public UTT()
    {
        Source = new();
    }
    public UTT(GFF source)
    {
        Source = source;
    }
    public static UTT FromFile(string filepath)
    {
        return new(GFF.FromFile(filepath));
    }
    public static UTT FromBytes(byte[] bytes)
    {
        return new(GFF.FromBytes(bytes));
    }
    public static UTT FromStream(Stream stream)
    {
        return new(GFF.FromStream(stream));
    }

    /// <remarks>
    /// This is the value stored in the <c>Tag</c> field in the UTT.
    /// </remarks>
    public string Tag
    {
        get => Source.Root.GetString("Tag") ?? "";
        set => Source.Root.SetString("Tag", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>TemplateResRef</c> field in the UTT.
    /// </remarks>
    public ResRef ResRef
    {
        get => Source.Root.GetResRef("TemplateResRef") ?? "";
        set => Source.Root.SetResRef("TemplateResRef", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>LocalizedName</c> field in the UTT.
    /// </remarks>
    public LocalisedString Name
    {
        get => Source.Root.GetLocalisedString("LocalizedName") ?? new();
        set => Source.Root.SetLocalisedString("LocalizedName", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>AutoRemoveKey</c> field in the UTT.
    /// </remarks>
    public bool AutoRemoveKey
    {
        get => Source.Root.GetUInt8("AutoRemoveKey") != 0;
        set => Source.Root.SetUInt8("AutoRemoveKey", Convert.ToByte(value));
    }

    /// <remarks>
    /// This is the value stored in the <c>Faction</c> field in the UTT.
    /// </remarks>
    public uint FactionID
    {
        get => Source.Root.GetUInt32("Faction") ?? 0;
        set => Source.Root.SetUInt32("Faction", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Cursor</c> field in the UTT.
    /// </remarks>
    public byte CursorID
    {
        get => Source.Root.GetUInt8("Cursor") ?? 0;
        set => Source.Root.SetUInt8("Cursor", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>HighlightHeight</c> field in the UTT.
    /// </remarks>
    public float HighlightHeight
    {
        get => Source.Root.GetSingle("HighlightHeight") ?? 0.0f;
        set => Source.Root.SetSingle("HighlightHeight", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>KeyName</c> field in the UTT.
    /// </remarks>
    public string KeyName
    {
        get => Source.Root.GetString("KeyName") ?? "";
        set => Source.Root.SetString("KeyName", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Type</c> field in the UTT.
    /// </remarks>
    public int Type
    {
        get => Source.Root.GetInt32("Type") ?? 0;
        set => Source.Root.SetInt32("Type", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>TrapDetectable</c> field in the UTT.
    /// </remarks>
    public bool TrapDetectable
    {
        get => Source.Root.GetUInt8("TrapDetectable") != 0;
        set => Source.Root.SetUInt8("TrapDetectable", Convert.ToByte(value));
    }

    /// <remarks>
    /// This is the value stored in the <c>TrapDetectDC</c> field in the UTT.
    /// </remarks>
    public byte TrapDetectDC
    {
        get => Source.Root.GetUInt8("TrapDetectDC") ?? 0;
        set => Source.Root.SetUInt8("TrapDetectDC", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>TrapDisarmable</c> field in the UTT.
    /// </remarks>
    public bool TrapDisarmable
    {
        get => Source.Root.GetUInt8("TrapDisarmable") != 0;
        set => Source.Root.SetUInt8("TrapDisarmable", Convert.ToByte(value));
    }

    /// <remarks>
    /// This is the value stored in the <c>DisarmDC</c> field in the UTT.
    /// </remarks>
    public byte DisarmDC
    {
        get => Source.Root.GetUInt8("DisarmDC") ?? 0;
        set => Source.Root.SetUInt8("DisarmDC", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>TrapFlag</c> field in the UTT.
    /// </remarks>
    public bool TrapFlag
    {
        get => Source.Root.GetUInt8("TrapFlag") != 0;
        set => Source.Root.SetUInt8("TrapFlag", Convert.ToByte(value));
    }

    /// <remarks>
    /// This is the value stored in the <c>TrapOneShot</c> field in the UTT.
    /// </remarks>
    public bool TrapOneShot
    {
        get => Source.Root.GetUInt8("TrapOneShot") != 0;
        set => Source.Root.SetUInt8("TrapOneShot", Convert.ToByte(value));
    }

    /// <remarks>
    /// This is the value stored in the <c>TrapType</c> field in the UTT.
    /// </remarks>
    public byte TrapType
    {
        get => Source.Root.GetUInt8("TrapType") ?? 0;
        set => Source.Root.SetUInt8("TrapType", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>OnDisarm</c> field in the UTT.
    /// </remarks>
    public ResRef OnDisarm
    {
        get => Source.Root.GetResRef("OnDisarm") ?? "";
        set => Source.Root.SetResRef("OnDisarm", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>OnTrapTriggered</c> field in the UTT.
    /// </remarks>
    public ResRef OnTrapTriggered
    {
        get => Source.Root.GetResRef("OnTrapTriggered") ?? "";
        set => Source.Root.SetResRef("OnTrapTriggered", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>OnClick</c> field in the UTT.
    /// </remarks>
    public ResRef OnClick
    {
        get => Source.Root.GetResRef("OnClick") ?? "";
        set => Source.Root.SetResRef("OnClick", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>ScriptHeartbeat</c> field in the UTT.
    /// </remarks>
    public ResRef OnHeartbeat
    {
        get => Source.Root.GetResRef("ScriptHeartbeat") ?? "";
        set => Source.Root.SetResRef("ScriptHeartbeat", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>ScriptOnEnter</c> field in the UTT.
    /// </remarks>
    public ResRef OnEnter
    {
        get => Source.Root.GetResRef("ScriptOnEnter") ?? "";
        set => Source.Root.SetResRef("ScriptOnEnter", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>ScriptOnExit</c> field in the UTT.
    /// </remarks>
    public ResRef OnExit
    {
        get => Source.Root.GetResRef("ScriptOnExit") ?? "";
        set => Source.Root.SetResRef("ScriptOnExit", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>ScriptUserDefine</c> field in the UTT.
    /// </remarks>
    public ResRef OnUserDefined
    {
        get => Source.Root.GetResRef("ScriptUserDefine") ?? "";
        set => Source.Root.SetResRef("ScriptUserDefine", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>PaletteID</c> field in the UTT.
    /// </remarks>
    public byte PaletteID
    {
        get => Source.Root.GetUInt8("PaletteID") ?? 0;
        set => Source.Root.SetUInt8("PaletteID", value);
    }

    /// <remarks>
    /// This is the value stored in the <c>Comment</c> field in the UTT.
    /// </remarks>
    public string Comment
    {
        get => Source.Root.GetString("Comment") ?? "";
        set => Source.Root.SetString("Comment", value);
    }
}
