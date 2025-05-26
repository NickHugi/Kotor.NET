namespace Kotor.NET.Resources.KotorTXI;

public class TXIBumpmap
{
    /// <summary>
    /// Marks the associated texture as a bumpmap.
    /// </summary>
    /// <remarks>
    /// The <c>isbumpmap</c> field in the TXI.
    /// </remarks>
    public bool? Enabled { get; set; }

    /// <summary>
    /// Very rarely used compared to scaling <c>Scaling</c> but possibly has the same affect?
    /// </summary>
    /// <remarks>
    /// The <c>bumpmapscale</c> field in the TXI.
    /// </remarks>
    public decimal? Scale { get; set; }

    /// <summary>
    /// Increases the intensity of the bump/normal map.
    /// </summary>
    /// <remarks>
    /// The <c>bumpmapscaling</c> in the TXI.
    /// </remarks>
    public decimal? Scaling { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>isdiffusebumpmap</c> in the TXI.
    /// </remarks>
    public bool? IsDiffuseBumpMap { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>isspecularbumpmap</c> field in the TXI.
    /// </remarks>
    public bool? IsSpecularBumpMap { get; set; }
}
