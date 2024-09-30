using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorARE;

public class AREMap
{
    private GFF _source;
    private GFFStruct? _struct => _source.Root.GetStruct("Map");

    public AREMap(GFF source)
    {
        _source = source;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>MapPt1X</c> field.
    /// </remarks>
    public float MapPoint1X
    {
        get => _struct.GetSingle("MapPt1X") ?? 0;
        set => _struct.SetSingle("MapPt1X", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>MapPt1Y</c> field.
    /// </remarks>
    public float MapPoint1Y
    {
        get => _struct.GetSingle("MapPt1Y") ?? 0;
        set => _struct.SetSingle("MapPt1Y", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>MapPt2X</c> field.
    /// </remarks>
    public float MapPoint2X
    {
        get => _struct.GetSingle("MapPt2X") ?? 0;
        set => _struct.SetSingle("MapPt2X", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>MapPt2Y</c> field.
    /// </remarks>
    public float MapPoint2Y
    {
        get => _struct.GetSingle("MapPt2Y") ?? 0;
        set => _struct.SetSingle("MapPt2Y", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>WorldPt1X</c> field.
    /// </remarks>
    public float WorldPoint1X
    {
        get => _struct.GetSingle("WorldPt1X") ?? 0;
        set => _struct.SetSingle("WorldPt1X", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>WorldPt1Y</c> field.
    /// </remarks>
    public float WorldPoint1Y
    {
        get => _struct.GetSingle("WorldPt1Y") ?? 0;
        set => _struct.SetSingle("WorldPt1Y", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>WorldPt2X</c> field.
    /// </remarks>
    public float WorldPoint2X
    {
        get => _struct.GetSingle("WorldPt2X") ?? 0;
        set => _struct.SetSingle("WorldPt2X", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>WorldPt2Y</c> field.
    /// </remarks>
    public float WorldPoint2Y
    {
        get => _struct.GetSingle("WorldPt2Y") ?? 0;
        set => _struct.SetSingle("WorldPt2Y", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>MapZoom</c> field.
    /// </remarks>
    public int MapZoom
    {
        get => _struct.GetInt32("MapZoom") ?? 0;
        set => _struct.SetInt32("MapZoom", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>NorthAxis</c> field.
    /// </remarks>
    public int NorthAxis
    {
        get => _struct.GetInt32("NorthAxis") ?? 0;
        set => _struct.SetInt32("NorthAxis", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>MapResX</c> field.
    /// </remarks>
    public int MapResX
    {
        get => _struct.GetInt32("MapResX") ?? 0;
        set => _struct.SetInt32("MapResX", value);
    }
}
