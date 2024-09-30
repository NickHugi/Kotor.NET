namespace Kotor.NET.Resources.KotorTXI;

public class TXIMaterial
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>alphamean</c> field in the TXI.
    /// </remarks>
    public decimal? AlphaMean { get; set; }

    /// <summary>
    /// Sets the normal map that will be applied to the texture.
    /// </summary>
    /// <remarks>
    /// The <c>bumpmaptexture</c> field in the TXI.
    /// </remarks>
    public string? BumpmapTexture { get; set; }

    /// <summary>
    /// Sets the environment map (cubemap) that will be applied to the texture.
    /// </summary>
    /// <remarks>
    /// The <c>bumpyshinytexture</c> field in the TXI.
    /// </remarks>
    public string? BumpyShinyTexture { get; set; }

    /// <summary>
    /// Sets the environment map (cubemap) that will be applied to the texture.
    /// </summary>
    /// <remarks>
    /// The <c>envmaptexture</c> field in the TXI.
    /// </remarks>
    public string? EnvironmentMapTexture { get; set; }

    /// <summary>
    /// How transparency is handled by the texture.
    /// </summary>
    /// <remarks>
    /// The <c>blending</c> field in the TXI.
    /// </remarks>
    public TXIBlending? Blending { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>wateralpha</c> field in the TXI.
    /// </remarks>
    public decimal? WaterAlpha { get; set; }
}
