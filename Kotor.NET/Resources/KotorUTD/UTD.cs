using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTD;

public class UTD
{
    public GFF Source { get; }

    public UTD()
    {
        Source = new();
    }
    public UTD(GFF source)
    {
        Source = source;
    }
    public static UTD FromFile(string filepath)
    {
        return new(GFF.FromFile(filepath));
    }
    public static UTD FromBytes(byte[] bytes)
    {
        return new(GFF.FromBytes(bytes));
    }
    public static UTD FromStream(Stream stream)
    {
        return new(GFF.FromStream(stream));
    }

    /// <summary>
    /// This is the value stored in the <c>AnimationState</c> field in the UTD.
    /// </summary>
    public byte AnimationState
    {
        get => Source.Root.GetUInt8("AnimationState") ?? 0;
        set => Source.Root.SetUInt8("AnimationState", value);
    }

    /// <summary>
    /// This is the value stored in the <c>AutoRemoveKey</c> field in the UTD.
    /// </summary>
    public bool AutoRemoveKey
    {
        get => Source.Root.GetUInt8("AutoRemoveKey") != 0;
        set => Source.Root.SetUInt8("AutoRemoveKey", Convert.ToByte(value));
    }

    /// <summary>
    /// This is the value stored in the <c>CloseLockDC</c> field in the UTD.
    /// </summary>
    public byte CloseLockDC
    {
        get => Source.Root.GetUInt8("CloseLockDC") ?? 0;
        set => Source.Root.SetUInt8("CloseLockDC", value);
    }

    /// <summary>
    /// This is the value stored in the <c>Comment</c> field in the UTD.
    /// </summary>
    public string Comment
    {
        get => Source.Root.GetString("Comment") ?? "";
        set => Source.Root.SetString("Comment", value);
    }

    /// <summary>
    /// This is the value stored in the <c>Conversation</c> field in the UTD.
    /// </summary>
    public ResRef Conversation
    {
        get => Source.Root.GetResRef("Conversation") ?? "";
        set => Source.Root.SetResRef("Conversation", value);
    }

    /// <summary>
    /// This is the value stored in the <c>CurrentHP</c> field in the UTD.
    /// </summary>
    public short CurrentHP
    {
        get => Source.Root.GetInt16("CurrentHP") ?? 0;
        set => Source.Root.SetInt16("CurrentHP", value);
    }

    /// <summary>
    /// This is the value stored in the <c>DisarmDC</c> field in the UTD.
    /// </summary>
    public byte DisarmDC
    {
        get => Source.Root.GetUInt8("DisarmDC") ?? 0;
        set => Source.Root.SetUInt8("DisarmDC", value);
    }

    /// <summary>
    /// This is the value stored in the <c>Faction</c> field in the UTD.
    /// </summary>
    public uint FactionID
    {
        get => Source.Root.GetUInt32("Faction") ?? 0;
        set => Source.Root.SetUInt32("Faction", value);
    }

    /// <summary>
    /// This is the value stored in the <c>Fort</c> field in the UTD.
    /// </summary>
    public byte Fortitude
    {
        get => Source.Root.GetUInt8("Fort") ?? 0;
        set => Source.Root.SetUInt8("Fort", value);
    }

    /// <summary>
    /// This is the value stored in the <c>GenericType</c> field in the UTD. This is an index into <c>genericdoors.2da</c> file.
    /// </summary>
    public byte AppearanceID
    {
        get => Source.Root.GetUInt8("GenericType") ?? 0;
        set => Source.Root.SetUInt8("GenericType", value);
    }

    /// <summary>
    /// This is the value stored in the <c>Hardness</c> field in the UTD.
    /// </summary>
    public byte Hardness
    {
        get => Source.Root.GetUInt8("Hardness") ?? 0;
        set => Source.Root.SetUInt8("Hardness", value);
    }

    /// <summary>
    /// This is the value stored in the <c>HP</c> field in the UTD.
    /// </summary>
    public short HP
    {
        get => Source.Root.GetInt16("HP") ?? 0;
        set => Source.Root.SetInt16("HP", value);
    }

    /// <summary>
    /// This is the value stored in the <c>Interruptable</c> field in the UTD.
    /// </summary>
    public bool Interruptable
    {
        get => Source.Root.GetUInt8("Interruptable") != 0;
        set => Source.Root.SetUInt8("Interruptable", Convert.ToByte(value));
    }

    /// <summary>
    /// This is the value stored in the <c>KeyName</c> field in the UTD.
    /// </summary>
    public string KeyName
    {
        get => Source.Root.GetString("KeyName") ?? "";
        set => Source.Root.SetString("KeyName", value);
    }

    /// <summary>
    /// This is the value stored in the <c>KeyRequired</c> field in the UTD.
    /// </summary>
    public bool KeyRequired
    {
        get => Source.Root.GetUInt8("KeyRequired") != 0;
        set => Source.Root.SetUInt8("KeyRequired", Convert.ToByte(value));
    }

    /// <summary>
    /// This is the value stored in the <c>Lockable</c> field in the UTD.
    /// </summary>
    public bool Lockable
    {
        get => Source.Root.GetUInt8("Lockable") != 0;
        set => Source.Root.SetUInt8("Lockable", Convert.ToByte(value));
    }

    /// <summary>
    /// This is the value stored in the <c>Locked</c> field in the UTD.
    /// </summary>
    public bool Locked
    {
        get => Source.Root.GetUInt8("Locked") != 0;
        set => Source.Root.SetUInt8("Locked", Convert.ToByte(value));
    }

    /// <summary>
    /// This is the value stored in the <c>LocName</c> field in the UTD.
    /// </summary>
    public LocalisedString Name
    {
        get => Source.Root.GetLocalisedString("LocName") ?? new();
        set => Source.Root.SetLocalisedString("LocName", value);
    }

    /// <summary>
    /// This is the value stored in the <c>Min1HP</c> field in the UTD.
    /// </summary>
    public bool Min1HP
    {
        get => Source.Root.GetUInt8("Min1HP") != 0;
        set => Source.Root.SetUInt8("Min1HP", Convert.ToByte(value));
    }

    /// <summary>
    /// This is the value stored in the <c>NotBlastable</c> field in the UTD. Only supported by KotOR 2.
    /// </summary>
    public bool NotBlastable
    {
        get => Source.Root.GetUInt8("NotBlastable") != 0;
        set => Source.Root.SetUInt8("NotBlastable", Convert.ToByte(value));
    }

    /// <summary>
    /// This is the value stored in the <c>OnClick</c> field in the UTD.
    /// </summary>
    public ResRef OnClick
    {
        get => Source.Root.GetResRef("OnClick") ?? "";
        set => Source.Root.SetResRef("OnClick", value);
    }

    /// <summary>
    /// This is the value stored in the <c>OnClosed</c> field in the UTD.
    /// </summary>
    public ResRef OnClosed
    {
        get => Source.Root.GetResRef("OnClosed") ?? "";
        set => Source.Root.SetResRef("OnClosed", value);
    }

    /// <summary>
    /// This is the value stored in the <c>OnDamaged</c> field in the UTD.
    /// </summary>
    public ResRef OnDamaged
    {
        get => Source.Root.GetResRef("OnDamaged") ?? "";
        set => Source.Root.SetResRef("OnDamaged", value);
    }

    /// <summary>
    /// This is the value stored in the <c>OnDeath</c> field in the UTD.
    /// </summary>
    public ResRef OnDeath
    {
        get => Source.Root.GetResRef("OnDeath") ?? "";
        set => Source.Root.SetResRef("OnDeath", value);
    }

    /// <summary>
    /// This is the value stored in the <c>OnFailToOpen</c> field in the UTD.
    /// </summary>
    public ResRef OnFailToOpen
    {
        get => Source.Root.GetResRef("OnFailToOpen") ?? "";
        set => Source.Root.SetResRef("OnFailToOpen", value);
    }

    /// <summary>
    /// This is the value stored in the <c>OnHeartbeat</c> field in the UTD.
    /// </summary>
    public ResRef OnHeartbeat
    {
        get => Source.Root.GetResRef("OnHeartbeat") ?? "";
        set => Source.Root.SetResRef("OnHeartbeat", value);
    }

    /// <summary>
    /// This is the value stored in the <c>OnMeleeAttacked</c> field in the UTD.
    /// </summary>
    public ResRef OnMeleeAttacked
    {
        get => Source.Root.GetResRef("OnMeleeAttacked") ?? "";
        set => Source.Root.SetResRef("OnMeleeAttacked", value);
    }

    /// <summary>
    /// This is the value stored in the <c>OnOpen</c> field in the UTD.
    /// </summary>
    public ResRef OnOpen
    {
        get => Source.Root.GetResRef("OnOpen") ?? "";
        set => Source.Root.SetResRef("OnOpen", value);
    }

    /// <summary>
    /// This is the value stored in the <c>OnUnlock</c> field in the UTD.
    /// </summary>
    public ResRef OnUnlock
    {
        get => Source.Root.GetResRef("OnUnlock") ?? "";
        set => Source.Root.SetResRef("OnUnlock", value);
    }

    /// <summary>
    /// This is the value stored in the <c>OnUserDefined</c> field in the UTD.
    /// </summary>
    public ResRef OnUserDefined
    {
        get => Source.Root.GetResRef("OnUserDefined") ?? "";
        set => Source.Root.SetResRef("OnUserDefined", value);
    }

    /// <summary>
    /// This is the value stored in the <c>OpenLockDC</c> field in the UTD.
    /// </summary>
    public byte OpenLockDC
    {
        get => Source.Root.GetUInt8("OpenLockDC") ?? 0;
        set => Source.Root.SetUInt8("OpenLockDC", value);
    }

    /// <summary>
    /// This is the value stored in the <c>OpenLockDiff</c> field in the UTD. Only supported by KotOR 2.
    /// </summary>
    public byte OpenLockDifficulty
    {
        get => Source.Root.GetUInt8("OpenLockDiff") ?? 0;
        set => Source.Root.SetUInt8("OpenLockDiff", value);
    }

    /// <summary>
    /// This is the value stored in the <c>OpenLockDiffMod</c> field in the UTD. Only supported by KotOR 2.
    /// </summary>
    public sbyte OpenLockDifficultyModifier
    {
        get => Source.Root.GetInt8("OpenLockDiffMod") ?? 0;
        set => Source.Root.SetInt8("OpenLockDiffMod", value);
    }

    /// <summary>
    /// This is the value stored in the <c>OpenState</c> field in the UTD. Only supported by KotOR 2.
    /// </summary>
    public byte OpenState
    {
        get => Source.Root.GetUInt8("OpenState") ?? 0;
        set => Source.Root.SetUInt8("OpenState", value);
    }

    /// <summary>
    /// This is the value stored in the <c>PaletteID</c> field in the UTD.
    /// </summary>
    public byte PaletteID
    {
        get => Source.Root.GetUInt8("PaletteID") ?? 0;
        set => Source.Root.SetUInt8("PaletteID", value);
    }

    /// <summary>
    /// This is the value stored in the <c>Plot</c> field in the UTD.
    /// </summary>
    public bool Plot
    {
        get => Source.Root.GetUInt8("Plot") != 0;
        set => Source.Root.SetUInt8("Plot", Convert.ToByte(value));
    }

    /// <summary>
    /// This is the value stored in the <c>Ref</c> field in the UTD.
    /// </summary>
    public byte Reflex
    {
        get => Source.Root.GetUInt8("Ref") ?? 0;
        set => Source.Root.SetUInt8("Ref", value);
    }

    /// <summary>
    /// This is the value stored in the <c>Static</c> field in the UTD.
    /// </summary>
    public bool Static
    {
        get => Source.Root.GetUInt8("Static") != 0;
        set => Source.Root.SetUInt8("Static", Convert.ToByte(value));
    }

    /// <summary>
    /// This is the value stored in the <c>Tag</c> field in the UTD.
    /// </summary>
    public string Tag
    {
        get => Source.Root.GetString("Tag") ?? "";
        set => Source.Root.SetString("Tag", value);
    }

    /// <summary>
    /// This is the value stored in the <c>TemplateResRef</c> field in the UTD.
    /// </summary>
    public ResRef ResRef
    {
        get => Source.Root.GetResRef("TemplateResRef") ?? "";
        set => Source.Root.SetResRef("TemplateResRef", value);
    }
}
