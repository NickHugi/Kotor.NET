namespace Kotor.NET.Resources.KotorTXI;

public class TXIFont
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>baselineheight</c> field in the TXI.
    /// </remarks>
    public decimal? BaselineHeight { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>numchars</c> field in the TXI.
    /// </remarks>
    public int? NumberOfCharacters { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>fontheight</c> field in the TXI.
    /// </remarks>
    public decimal? FontHeight { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>texturewidth</c> field in the TXI.
    /// </remarks>
    public decimal? TextureWidth { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>spacingB</c> field in the TXI.
    /// </remarks>
    public decimal? SpacingBottom { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>spacingR</c> field in the TXI.
    /// </remarks>
    public decimal? SpacingRight { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>caretindent</c> field in the TXI.
    /// </remarks>
    public decimal? CaretIndent { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>upperleftcoords</c> and <c>lowerrightcoords</c> fields in the TXI.
    /// </remarks>
    public List<TXIFontCoords>? Coords { get; set; }
}
