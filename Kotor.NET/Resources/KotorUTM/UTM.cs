using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;
using Kotor.NET.Resources.KotorUTC;

namespace Kotor.NET.Resources.KotorUTM;

public class UTM
{
    public GFF Source { get; }

    public UTM()
    {
        Source = new();
    }
    public UTM(GFF source)
    {
        Source = source;
    }
    public static UTM FromFile(string filepath)
    {
        return new(GFF.FromFile(filepath));
    }
    public static UTM FromBytes(byte[] bytes)
    {
        return new(GFF.FromBytes(bytes));
    }
    public static UTM FromStream(Stream stream)
    {
        return new(GFF.FromStream(stream));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c></c> field in the UTM.
    /// </remarks>
    public ResRef ResRef
    {
        get => Source.Root.GetResRef("ResRef") ?? "";
        set => Source.Root.SetResRef("ResRef", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>LocName</c> field in the UTM.
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
    /// This is the value stored in the <c>Tag</c> field in the UTM.
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
    /// This is the value stored in the <c>MarkUp</c> field in the UTM.
    /// </remarks>
    public int MarkUp
    {
        get => Source.Root.GetInt32("MarkUp") ?? 0;
        set => Source.Root.SetInt32("MarkUp", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>MarkDown</c> field in the UTM.
    /// </remarks>
    public int MarkDown
    {
        get => Source.Root.GetInt32("MarkDown") ?? 0;
        set => Source.Root.SetInt32("MarkDown", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnOpenStore</c> field in the UTM.
    /// </remarks>
    public ResRef OnOpenStore
    {
        get => Source.Root.GetResRef("OnOpenStore") ?? "";
        set => Source.Root.SetResRef("OnOpenStore", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ID</c> field in the UTM.
    /// </remarks>
    public byte ID
    {
        get => Source.Root.GetUInt8("ID") ?? 0;
        set => Source.Root.SetUInt8("ID", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Comment</c> field in the UTM.
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
    /// This is the value stored in the <c>BuySellFlag</c> field in the UTM and is toggled by the 1st bit.
    /// </remarks>
    public bool CanBuy
    {
        get => (Source.Root.GetUInt8("BuySellFlag") & 0b01) != 0;
        set
        {
            var canSell = (Source.Root.GetUInt8("BuySellFlag") ?? 0) & 0b10;
            var alter = canSell | (Convert.ToByte(value) * 0b01);
            Source.Root.SetUInt8("BuySellFlag", (byte)alter);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>BuySellFlag</c> field in the UTM and is toggled by the 2nd bit.
    /// </remarks>
    public bool CanSell
    {
        get => (Source.Root.GetUInt8("BuySellFlag") & 0b10) != 0;
        set
        {
            var canBuy = (Source.Root.GetUInt8("BuySellFlag") ?? 0) & 0b01;
            var alter = canBuy | (Convert.ToByte(value) * 0b10);
            Source.Root.SetUInt8("BuySellFlag", (byte)alter);
        }
    }

    public UTMItemCollection Items => new(Source);
}
