using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorGUI;

public class GUI
{
    
}

public class GUIControl
{
    private GFF _source { get; }
    private GFFStruct _controlSource { get; }

    // Obj_Parent
    // Obj_ParentID

    public string Tag
    {
        get => _controlSource.GetString("TAG") ?? "";
        set => _controlSource.SetString("TAG", value);
    }

    public bool Locked
    {
        get => _controlSource.GetUInt8("Obj_Locked") > 0;
        set => _controlSource.SetUInt8("Obj_Locked", Convert.ToByte(value));
    }

    public float Alpha
    {
        get => _controlSource.GetSingle("ALPHA") ?? 0.0f;
        set => _controlSource.SetSingle("ALPHA", value);
    }

    public Common.Data.Vector3 Color
    {
        get => _controlSource.GetVector3("COLOR") ?? new();
        set => _controlSource.SetVector3("COLOR", value);
    }
}

public class GUIExtent
{
    private GFF _source { get; }
    private GFFStruct _extentSource { get; }

    public int Left
    {
        get => _extentSource.GetInt32("LEFT") ?? 0;
        set => _extentSource.SetInt32("LEFT", value);
    }

    public int Top
    {
        get => _extentSource.GetInt32("TOP") ?? 0;
        set => _extentSource.SetInt32("TOP", value);
    }

    public int Width
    {
        get => _extentSource.GetInt32("WIDTH") ?? 0;
        set => _extentSource.SetInt32("WIDTH", value);
    }

    public int Height
    {
        get => _extentSource.GetInt32("HEIGHT") ?? 0;
        set => _extentSource.SetInt32("HEIGHT", value);
    }
}
