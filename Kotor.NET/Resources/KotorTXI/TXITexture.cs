namespace Kotor.NET.Resources.KotorTXI;

public class TXITexture
{
    /// <summary>
    /// Marks the texture as a cube map.
    /// </summary>
    /// <remarks>
    /// The <c>cube</c> field in the TXI.
    /// </remarks>
    public bool? IsCubemap { get; set; }

    /// <summary>
    /// Marks the texture as a lightmap.
    /// </summary>
    /// <remarks>
    /// The <c>islightmap</c> field in the TXI.
    /// </remarks>
    public bool? IsLightmap { get; set; }

    /// <summary>
    /// Used to prevent Z-fighting on polygons.
    /// </summary>
    /// <remarks>
    /// The <c>decal</c> field in the TXI.
    /// </remarks>
    public bool? IsDecal { get; set; }

    /// <summary>
    /// Not used in game but rather the official development tools of the game engine.
    /// </summary>
    /// <remarks>
    /// The <c>downsamplemin</c> field of the TXI.
    /// </remarks>
    public int? DownsampleMin { get; set; }

    /// <summary>
    /// Not used in game but rather the official development tools of the game engine.
    /// </summary>
    /// <remarks>
    /// The <c>downsamplemax</c> field of the TXI.
    /// </remarks>
    public int? DownsampleMax { get; set; }

    /// <summary>
    /// Specifies whether the texture will use mipmapping.
    /// </summary>
    /// <remarks>>
    /// The <c>mipmap</c> field in the TXI.
    /// </remarks>
    public bool? EnableMipmaps { get; set; }

    /// <summary>
    /// Enables linear filtering on the texture. This will make the texture more smooth/blurry
    /// and less pixelated.
    /// </summary>
    /// <remarks>
    /// The <c>filter</c> field in the TXI.
    /// </remarks>
    public bool? UseLinearFiltering { get; set; }

    /// <summary>
    /// Stops the GPU from compressing the texture when loading into memory. Used for
    /// lightmaps to prevent artefacts.
    /// </summary>
    /// <remarks>
    /// The <c>compresstexture</c> field in the TXI.
    /// </remarks>
    public bool? CompressTexture { get; set; }

    /// <summary>
    /// Clamps texture coordinates to prevent the texture from tiling.
    /// </summary>
    /// <remarks>
    /// The <c>clamp</c> field in the TXI.
    /// </remarks>a
    public TXIClamping? Clamp { get; set; }
}
