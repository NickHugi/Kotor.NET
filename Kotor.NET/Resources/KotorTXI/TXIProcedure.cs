namespace Kotor.NET.Resources.KotorTXI;

public class TXIProcedure
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>proceduretype</c> field in the TXI.
    /// </remarks>
    public TXIProcedureType? Type { get; set; }

    /// <summary>
    /// Sets the width for the procedurally generated image.
    /// </summary>
    /// <remarks>
    /// The <c>defaultwidth</c> field of the TXI.
    /// </remarks>
    public int? DefaultWidth { get; set; }

    /// <summary>
    /// Sets the height for the procedurally generated image.
    /// </summary>
    /// <remarks>
    /// The <c>defaultheight</c> field of the TXI.
    /// </remarks>
    public int? DefaultHeight { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>numx</c> field of the TXI.
    /// </remarks>
    public int? NumFramesOnXAxis { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>numy</c> field of the TXI.
    /// </remarks>
    public int? NumFramesOnYAxis { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>fps</c> field of the TXI.
    /// </remarks>
    public int? FPS { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>speed</c> field of the TXI.
    /// </remarks>
    public decimal? Speed { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>arturowidth</c> field of the TXI.
    /// </remarks>
    public int? ArturoWidth { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>arturoheight</c> field of the TXI.
    /// </remarks>
    public int? ArturoHeight { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>distort</c> field of the TXI.
    /// </remarks>
    public decimal? Distort { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The <c>distortionamplitude</c> field of the TXI.
    /// </remarks>
    public decimal? DistortAmplitude { get; set; }

    public List<TXIChannelElement>? Channel { get; set; }
}
