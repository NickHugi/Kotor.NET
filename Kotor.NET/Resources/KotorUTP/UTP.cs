using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTP;

public class UTP
{
    public GFF Source { get; }

    public UTP()
    {
        Source = new();
    }

    public UTP(GFF source)
    {
        Source = source;
    }

    public static UTP FromFile(string filepath)
    {
        return new(GFF.FromFile(filepath));
    }

    public static UTP FromBytes(byte[] bytes)
    {
        return new(GFF.FromBytes(bytes));
    }

    public static UTP FromStream(Stream stream)
    {
        return new(GFF.FromStream(stream));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>AnimationState</c> field in the UTP.
    /// </remarks>
    public byte AnimationState
    {
        get => Source.Root.GetUInt8("AnimationState") ?? 0;
        set => Source.Root.SetUInt8("AnimationState", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Appearance</c> field in the UTP and is an index into
    /// the <c>placeables.2da</c> file.
    /// </remarks>
    public uint AppearanceID
    {
        get => Source.Root.GetUInt32("Appearance") ?? 0;
        set => Source.Root.SetUInt32("Appearance", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>AutoRemoveKey</c> field in the UTP.
    /// </remarks>
    public bool AutoRemoveKey
    {
        get => Source.Root.GetUInt8("AutoRemoveKey") != 0;
        set => Source.Root.SetUInt8("AutoRemoveKey", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Comment</c> field in the UTP.
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
    /// This is the value stored in the <c>Conversation</c> field in the UTP.
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
    /// This is the value stored in the <c>CurrentHP</c> field in the UTP.
    /// </remarks>
    public short CurrentHP
    {
        get => Source.Root.GetInt16("CurrentHP") ?? 0;
        set => Source.Root.SetInt16("CurrentHP", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Faction</c> field in the UTP.
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
    /// This is the value stored in the <c>Fort</c> field in the UTP.
    /// </remarks>
    public byte Fortitude
    {
        get => Source.Root.GetUInt8("Fort") ?? 0;
        set => Source.Root.SetUInt8("Fort", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Hardness</c> field in the UTP.
    /// </remarks>
    public byte Hardness
    {
        get => Source.Root.GetUInt8("Hardness") ?? 0;
        set => Source.Root.SetUInt8("Hardness", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>HasInventory</c> field in the UTP.
    /// </remarks>
    public bool HasInventory
    {
        get => Source.Root.GetUInt8("HasInventory") != 0;
        set => Source.Root.SetUInt8("HasInventory", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>HP</c> field in the UTP.
    /// </remarks>
    public short HP
    {
        get => Source.Root.GetInt16("HP") ?? 0;
        set => Source.Root.SetInt16("HP", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>IsComputer</c> field in the UTP.
    /// </remarks>
    public bool IsComputer
    {
        get => (Source.Root.GetUInt8("IsComputer") ?? 0) != 0;
        set => Source.Root.SetUInt8("IsComputer", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>KeyName</c> field in the UTP.
    /// </remarks>
    public string KeyName
    {
        get => Source.Root.GetString("KeyName") ?? "";
        set => Source.Root.SetString("KeyName", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>KeyRequired</c> field in the UTP.
    /// </remarks>
    public bool KeyRequired
    {
        get => Source.Root.GetUInt8("KeyRequired") != 0;
        set => Source.Root.SetUInt8("KeyRequired", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Lockable</c> field in the UTP.
    /// </remarks>
    public bool Lockable
    {
        get => Source.Root.GetUInt8("Lockable") != 0;
        set => Source.Root.SetUInt8("Lockable", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Locked</c> field in the UTP.
    /// </remarks>
    public bool Locked
    {
        get => Source.Root.GetUInt8("Locked") != 0;
        set => Source.Root.SetUInt8("Locked", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>LocName</c> field in the UTP.
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
    /// This is the value stored in the <c>Min1HP</c> field in the UTP.
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
    /// This is the value stored in the <c>NotBlastable</c> field in the UTP. Only supported by KotOR 2.
    /// </remarks>
    public bool NotBlastable
    {
        get => Source.Root.GetUInt8("NotBlastable") != 0;
        set => Source.Root.SetUInt8("NotBlastable", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnClosed</c> field in the UTP.
    /// </remarks>
    public ResRef OnClosed
    {
        get => Source.Root.GetResRef("OnClosed") ?? "";
        set => Source.Root.SetResRef("OnClosed", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnDamaged</c> field in the UTP.
    /// </remarks>
    public ResRef OnDamaged
    {
        get => Source.Root.GetResRef("OnDamaged") ?? "";
        set => Source.Root.SetResRef("OnDamaged", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnDeath</c> field in the UTP.
    /// </remarks>
    public ResRef OnDeath
    {
        get => Source.Root.GetResRef("OnDeath") ?? "";
        set => Source.Root.SetResRef("OnDeath", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnEndDialogue</c> field in the UTP.
    /// </remarks>
    public ResRef OnEndDialogue
    {
        get => Source.Root.GetResRef("OnEndDialogue") ?? "";
        set => Source.Root.SetResRef("OnEndDialogue", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnFailToOpen</c> field in the UTP. Only supported by KotOR 2.
    /// </remarks>
    public ResRef OnFailToOpen
    {
        get => Source.Root.GetResRef("OnFailToOpen") ?? "";
        set => Source.Root.SetResRef("OnFailToOpen", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnHeartbeat</c> field in the UTP.
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
    /// This is the value stored in the <c>OnInvDisturbed</c> field in the UTP.
    /// </remarks>
    public ResRef OnInvDisturbed
    {
        get => Source.Root.GetResRef("OnInvDisturbed") ?? "";
        set => Source.Root.SetResRef("OnInvDisturbed", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnMeleeAttacked</c> field in the UTP.
    /// </remarks>
    public ResRef OnMeleeAttacked
    {
        get => Source.Root.GetResRef("OnMeleeAttacked") ?? "";
        set => Source.Root.SetResRef("OnMeleeAttacked", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnOpen</c> field in the UTP.
    /// </remarks>
    public ResRef OnOpen
    {
        get => Source.Root.GetResRef("OnOpen") ?? "";
        set => Source.Root.SetResRef("OnOpen", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnSpellCastAt</c> field in the UTP.
    /// </remarks>
    public ResRef OnSpellCastAt
    {
        get => Source.Root.GetResRef("OnSpellCastAt") ?? "";
        set => Source.Root.SetResRef("OnSpellCastAt", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnUnlock</c> field in the UTP.
    /// </remarks>
    public ResRef OnUnlock
    {
        get => Source.Root.GetResRef("OnUnlock") ?? "";
        set => Source.Root.SetResRef("OnUnlock", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnUsed</c> field in the UTP.
    /// </remarks>
    public ResRef OnUsed
    {
        get => Source.Root.GetResRef("OnUsed") ?? "";
        set => Source.Root.SetResRef("OnUsed", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnUserDefined</c> field in the UTP.
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
    /// This is the value stored in the <c>OpenLockDC</c> field in the UTP.
    /// </remarks>
    public byte OpenLockDC
    {
        get => Source.Root.GetUInt8("OpenLockDC") ?? 0;
        set => Source.Root.SetUInt8("OpenLockDC", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OpenLockDiff</c> field in the UTP. Only supported by KotOR 2.
    /// </remarks>
    public byte OpenLockDiff
    {
        get => Source.Root.GetUInt8("OpenLockDiff") ?? 0;
        set => Source.Root.SetUInt8("OpenLockDiff", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OpenLockDiffMod</c> field in the UTP. Only supported by KotOR 2.
    /// </remarks>
    public sbyte OpenLockDiffMod
    {
        get => Source.Root.GetInt8("OpenLockDiffMod") ?? 0;
        set => Source.Root.SetInt8("OpenLockDiffMod", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>PaletteID</c> field in the UTP.
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
    /// This is the value stored in the <c>PartyInteract</c> field in the UTP.
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
    /// This is the value stored in the <c>Plot</c> field in the UTP.
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
    /// This is the value stored in the <c>Ref</c> field in the UTP.
    /// </remarks>
    public byte Reflex
    {
        get => Source.Root.GetUInt8("Ref") ?? 0;
        set => Source.Root.SetUInt8("Ref", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Static</c> field in the UTP.
    /// </remarks>
    public bool Static
    {
        get => Source.Root.GetUInt8("Static") != 0;
        set => Source.Root.SetUInt8("Static", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Tag</c> field in the UTP.
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
    /// This is the value stored in the <c>TemplateResRef</c> field in the UTP.
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
    /// This is the value stored in the <c>Type</c> field in the UTP.
    /// </remarks>
    public byte Type
    {
        get => Source.Root.GetUInt8("Type") ?? 0;
        set => Source.Root.SetUInt8("Type", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Useable</c> field in the UTP.
    /// </remarks>
    public bool Useable
    {
        get => Source.Root.GetUInt8("Useable") != 0;
        set => Source.Root.SetUInt8("Useable", Convert.ToByte(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Will</c> field in the UTP.
    /// </remarks>
    public byte Will
    {
        get => Source.Root.GetUInt8("Will") ?? 0;
        set => Source.Root.SetUInt8("Will", value);
    }

    public UTPItemCollection Inventory => new UTPItemCollection(Source);
}

