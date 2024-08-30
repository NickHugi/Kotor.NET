using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorARE;

public class ARE
{
    public GFF Source { get; }

    public ARE()
    {
        Source = new();
    }
    public ARE(GFF source)
    {
        Source = source;
    }
    public static ARE FromFile(string filepath)
    {
        return new(GFF.FromFile(filepath));
    }
    public static ARE FromBytes(byte[] bytes)
    {
        return new(GFF.FromBytes(bytes));
    }
    public static ARE FromStream(Stream stream)
    {
        return new(GFF.FromStream(stream));
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>AlphaTest</c> field in the ARE.
    /// </remarks>
    public float AlphaTest
    {
        get => Source.Root.GetSingle("AlphaTest") ?? 0.0f;
        set => Source.Root.SetSingle("AlphaTest", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>CameraStyle</c> field in the ARE.
    /// </remarks>
    public int CameraStyle
    {
        get => Source.Root.GetInt32("CameraStyle") ?? 0;
        set => Source.Root.SetInt32("CameraStyle", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ChanceLightning</c> field in the ARE.
    /// </remarks>
    public int ChanceLightning
    {
        get => Source.Root.GetInt32("ChanceLightning") ?? 0;
        set => Source.Root.SetInt32("ChanceLightning", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ChanceRain</c> field in the ARE.
    /// </remarks>
    public int ChanceRain
    {
        get => Source.Root.GetInt32("ChanceRain") ?? 0;
        set => Source.Root.SetInt32("ChanceRain", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ChanceSnow</c> field in the ARE.
    /// </remarks>
    public int ChanceSnow
    {
        get => Source.Root.GetInt32("ChanceSnow") ?? 0;
        set => Source.Root.SetInt32("ChanceSnow", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Comments</c> field in the ARE.
    /// </remarks>
    public string Comments
    {
        get => Source.Root.GetString("Comments") ?? "";
        set => Source.Root.SetString("Comments", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>CreatorID</c> field in the ARE.
    /// </remarks>
    public int CreatorID
    {
        get => Source.Root.GetInt32("Creator_ID") ?? 0;
        set => Source.Root.SetInt32("Creator_ID", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DefaultEnvMap</c> field in the ARE.
    /// </remarks>
    public ResRef DefaultEnvironmentMap
    {
        get => Source.Root.GetResRef("DefaultEnvMap") ?? "";
        set => Source.Root.SetResRef("DefaultEnvMap", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DirtyARGBOne</c> field in the ARE. Only supported by KotOR 2.
    /// </remarks>
    public int DirtyARGBOne
    {
        get => Source.Root.GetInt32("DirtyARGBOne") ?? 0;
        set => Source.Root.SetInt32("DirtyARGBOne", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DirtyARGBTwo</c> field in the ARE. Only supported by KotOR 2.
    /// </remarks>
    public int DirtyARGBTwo
    {
        get => Source.Root.GetInt32("DirtyARGBTwo") ?? 0;
        set => Source.Root.SetInt32("DirtyARGBTwo", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DirtyARGBThree</c> field in the ARE. Only supported by KotOR 2.
    /// </remarks>
    public int DirtyARGBThree
    {
        get => Source.Root.GetInt32("DirtyARGBThree") ?? 0;
        set => Source.Root.SetInt32("DirtyARGBThree", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DirtyFormulaOne</c> field in the ARE. Only supported by KotOR 2.
    /// </remarks>
    public int DirtyFormulaOne
    {
        get => Source.Root.GetInt32("DirtyFormulaOne") ?? 0;
        set => Source.Root.SetInt32("DirtyFormulaOne", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DirtyFormulaTwo</c> field in the ARE. Only supported by KotOR 2.
    /// </remarks>
    public int DirtyFormulaTwo
    {
        get => Source.Root.GetInt32("DirtyFormulaTwo") ?? 0;
        set => Source.Root.SetInt32("DirtyFormulaTwo", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DirtyFormulaThree</c> field in the ARE. Only supported by KotOR 2.
    /// </remarks>
    public int DirtyFormulaThree
    {
        get => Source.Root.GetInt32("DirtyFormulaThree") ?? 0;
        set => Source.Root.SetInt32("DirtyFormulaThree", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DirtyFuncOne</c> field in the ARE. Only supported by KotOR 2.
    /// </remarks>
    public int DirtyFuncOne
    {
        get => Source.Root.GetInt32("DirtyFuncOne") ?? 0;
        set => Source.Root.SetInt32("DirtyFuncOne", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DirtyFuncTwo</c> field in the ARE. Only supported by KotOR 2.
    /// </remarks>
    public int DirtyFuncTwo
    {
        get => Source.Root.GetInt32("DirtyFuncTwo") ?? 0;
        set => Source.Root.SetInt32("DirtyFuncTwo", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DirtyFuncThree</c> field in the ARE. Only supported by KotOR 2.
    /// </remarks>
    public int DirtyFuncThree
    {
        get => Source.Root.GetInt32("DirtyFuncThree") ?? 0;
        set => Source.Root.SetInt32("DirtyFuncThree", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DirtySizeOne</c> field in the ARE. Only supported by KotOR 2.
    /// </remarks>
    public int DirtySizeOne
    {
        get => Source.Root.GetInt32("DirtySizeOne") ?? 0;
        set => Source.Root.SetInt32("DirtySizeOne", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DirtySizeTwo</c> field in the ARE. Only supported by KotOR 2.
    /// </remarks>
    public int DirtySizeTwo
    {
        get => Source.Root.GetInt32("DirtySizeTwo") ?? 0;
        set => Source.Root.SetInt32("DirtySizeTwo", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DirtySizeThree</c> field in the ARE. Only supported by KotOR 2.
    /// </remarks>
    public int DirtySizeThree
    {
        get => Source.Root.GetInt32("DirtySizeThree") ?? 0;
        set => Source.Root.SetInt32("DirtySizeThree", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DisableTransit</c> field in the ARE. Only supported by KotOR 2.
    /// </remarks>
    public bool DisableTransit
    {
        get => Source.Root.GetUInt8("DisableTransit") != 0;
        set => Source.Root.SetUInt8("DisableTransit", Convert.ToByte(value));
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DynAmbientColor</c> field in the ARE.
    /// </remarks>
    public uint DynAmbientColor
    {
        get => Source.Root.GetUInt32("DynAmbientColor") ?? 0;
        set => Source.Root.SetUInt32("DynAmbientColor", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Flags</c> field in the ARE.
    /// </remarks>
    public uint Flags
    {
        get => Source.Root.GetUInt32("Flags") ?? 0;
        set => Source.Root.SetUInt32("Flags", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>GrassAmbient</c> field in the ARE. Only supported by KotOR 2.
    /// </remarks>
    public uint GrassAmbient
    {
        get => Source.Root.GetUInt32("Grass_Ambient") ?? 0;
        set => Source.Root.SetUInt32("Grass_Ambient", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>GrassDensity</c> field in the ARE.
    /// </remarks>
    public float GrassDensity
    {
        get => Source.Root.GetSingle("Grass_Density") ?? 0.0f;
        set => Source.Root.SetSingle("Grass_Density", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>XXXXXXX</c> field in the ARE.
    /// </remarks>
    public uint GrassDiffuse
    {
        get => Source.Root.GetUInt32("Grass_Diffuse") ?? 0;
        set => Source.Root.SetUInt32("Grass_Diffuse", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>GrassEmissive</c> field in the ARE.
    /// </remarks>
    public uint GrassEmissive
    {
        get => Source.Root.GetUInt32("Grass_Emissive") ?? 0;
        set => Source.Root.SetUInt32("Grass_Emissive", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>GrassProbLL</c> field in the ARE.
    /// </remarks>
    public float GrassProbLL
    {
        get => Source.Root.GetSingle("Grass_Prob_LL") ?? 0.0f;
        set => Source.Root.SetSingle("Grass_Prob_LL", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>GrassProbLR</c> field in the ARE.
    /// </remarks>
    public float GrassProbLR
    {
        get => Source.Root.GetSingle("Grass_Prob_LR") ?? 0.0f;
        set => Source.Root.SetSingle("Grass_Prob_LR", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>GrassProbUL</c> field in the ARE.
    /// </remarks>
    public float GrassProbUL
    {
        get => Source.Root.GetSingle("Grass_Prob_UL") ?? 0.0f;
        set => Source.Root.SetSingle("Grass_Prob_UL", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>GrassProbUR</c> field in the ARE.
    /// </remarks>
    public float GrassProbUR
    {
        get => Source.Root.GetSingle("Grass_Prob_UR") ?? 0.0f;
        set => Source.Root.SetSingle("Grass_Prob_UR", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>GrassQuadSize</c> field in the ARE.
    /// </remarks>
    public float GrassQuadSize
    {
        get => Source.Root.GetSingle("Grass_QuadSize") ?? 0.0f;
        set => Source.Root.SetSingle("Grass_QuadSize", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>GrassTexName</c> field in the ARE.
    /// </remarks>
    public ResRef GrassTexture
    {
        get => Source.Root.GetResRef("Grass_TexName") ?? "";
        set => Source.Root.SetResRef("Grass_TexName", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ID</c> field in the ARE.
    /// </remarks>
    public int ID
    {
        get => Source.Root.GetInt32("ID") ?? 0;
        set => Source.Root.SetInt32("ID", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>IsNight</c> field in the ARE.
    /// </remarks>
    public bool IsNight
    {
        get => Source.Root.GetUInt8("IsNight") != 0;
        set => Source.Root.SetUInt8("IsNight", Convert.ToByte(value));
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>LightingScheme</c> field in the ARE.
    /// </remarks>
    public byte LightingScheme
    {
        get => Source.Root.GetUInt8("LightingScheme") ?? 0;
        set => Source.Root.SetUInt8("LightingScheme", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>LoadScreenID</c> field in the ARE.
    /// </remarks>
    public ushort LoadScreenID
    {
        get => Source.Root.GetUInt16("LoadScreenID") ?? 0;
        set => Source.Root.SetUInt16("LoadScreenID", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ModListenCheck</c> field in the ARE.
    /// </remarks>
    public int ModListenCheck
    {
        get => Source.Root.GetInt32("ModListenCheck") ?? 0;
        set => Source.Root.SetInt32("ModListenCheck", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ModSpotCheck</c> field in the ARE.
    /// </remarks>
    public int ModSpotCheck
    {
        get => Source.Root.GetInt32("ModSpotCheck") ?? 0;
        set => Source.Root.SetInt32("ModSpotCheck", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Name</c> field in the ARE.
    /// </remarks>
    public LocalisedString Name
    {
        get => Source.Root.GetLocalisedString("Name") ?? new();
        set => Source.Root.SetLocalisedString("Name", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnEnter</c> field in the ARE.
    /// </remarks>
    public ResRef OnEnter
    {
        get => Source.Root.GetResRef("OnEnter") ?? "";
        set => Source.Root.SetResRef("OnEnter", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>OnExit</c> field in the ARE.
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
    /// This is the value stored in the <c>OnHeartbeat</c> field in the ARE.
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
    /// This is the value stored in the <c>OnUserDefined</c> field in the ARE.
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
    /// This is the value stored in the <c>PlayerOnly</c> field in the ARE.
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
    /// This is the value stored in the <c>ShadowOpacity</c> field in the ARE.
    /// </remarks>
    public byte ShadowOpacity
    {
        get => Source.Root.GetUInt8("ShadowOpacity") ?? 0;
        set => Source.Root.SetUInt8("ShadowOpacity", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>StealthXPEnabled</c> field in the ARE.
    /// </remarks>
    public bool StealthXPEnabled
    {
        get => Source.Root.GetUInt8("StealthXPEnabled") != 0;
        set => Source.Root.SetUInt8("StealthXPEnabled", Convert.ToByte(value));
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>StealthXPLoss</c> field in the ARE.
    /// </remarks>
    public uint StealthXPLoss
    {
        get => Source.Root.GetUInt32("StealthXPLoss") ?? 0;
        set => Source.Root.SetUInt32("StealthXPLoss", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>StealthXPMax</c> field in the ARE.
    /// </remarks>
    public uint StealthXPMax
    {
        get => Source.Root.GetUInt32("StealthXPMax") ?? 0;
        set => Source.Root.SetUInt32("StealthXPMax", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SunAmbientColor</c> field in the ARE.
    /// </remarks>
    public uint SunAmbientColour
    {
        get => Source.Root.GetUInt32("SunAmbientColor") ?? 0;
        set => Source.Root.SetUInt32("SunAmbientColor", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SunDiffuseColor</c> field in the ARE.
    /// </remarks>
    public uint SunDiffuseColour
    {
        get => Source.Root.GetUInt32("SunDiffuseColor") ?? 0;
        set => Source.Root.SetUInt32("SunDiffuseColor", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SunFogColor</c> field in the ARE.
    /// </remarks>
    public uint SunFogColour
    {
        get => Source.Root.GetUInt32("SunFogColor") ?? 0;
        set => Source.Root.SetUInt32("SunFogColor", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SunFogFar</c> field in the ARE.
    /// </remarks>
    public float SunFogFar
    {
        get => Source.Root.GetSingle("SunFogFar") ?? 0.0f;
        set => Source.Root.SetSingle("SunFogFar", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SunFogNear</c> field in the ARE.
    /// </remarks>
    public float SunFogNear
    {
        get => Source.Root.GetSingle("SunFogNear") ?? 0.0f;
        set => Source.Root.SetSingle("SunFogNear", value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SunFogOn</c> field in the ARE.
    /// </remarks>
    public bool SunFogOn
    {
        get => Source.Root.GetUInt8("SunFogOn") != 0;
        set => Source.Root.SetUInt8("SunFogOn", Convert.ToByte(value));
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SunShadows</c> field in the ARE.
    /// </remarks>
    public bool SunShadows
    {
        get => Source.Root.GetUInt8("SunShadows") != 0;
        set => Source.Root.SetUInt8("SunShadows", Convert.ToByte(value));
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Tag</c> field in the ARE.
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
    /// This is the value stored in the <c>Unescapable</c> field in the ARE.
    /// </remarks>
    public bool Unescapable
    {
        get => Source.Root.GetUInt8("Unescapable") != 0;
        set => Source.Root.SetUInt8("Unescapable", Convert.ToByte(value));
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>WindPower</c> field in the ARE.
    /// </remarks>
    public int WindPower
    {
        get => Source.Root.GetInt32("WindPower") ?? 0;
        set => Source.Root.SetInt32("WindPower", value);
    }

    public AREMap Map => new AREMap(Source);
    public ARERoomCollection Rooms => new ARERoomCollection(Source);
}
