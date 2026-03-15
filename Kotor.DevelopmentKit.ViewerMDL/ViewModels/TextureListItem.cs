using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Kotor.NET.Formats.BinaryTPC;
using Kotor.NET.Formats.BinaryTPC.Serialisation;
using Kotor.NET.Resources.KotorTPC;
using ReactiveUI;

namespace Kotor.DevelopmentKit.ViewerMDL.ViewModels;

public class TextureListItem : ReactiveObject
{
    public string Name
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string Source
    {
        get => field;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
            this.RaisePropertyChanged(nameof(Image));
        }
    }

    public Bitmap Image
    {
        get
        {
            if (Source is null)
                return null;

            using var stream = File.OpenRead(Source);
            var tpc = new TPCBinaryDeserializer(new TPCBinary(stream)).Deserialize();

            return CreateBitmapFromRgb(tpc);
        }
    }

    public static Bitmap CreateBitmapFromRgb(TPC tpc)
    {
        var pixelFormat = PixelFormat.Rgb32;
        var alphaFormat = AlphaFormat.Opaque;

        int stride = tpc.Width * 4;

        unsafe
        {
            fixed (byte* ptr = tpc.GetData(0, 0))
            {
                return new Bitmap(
                    pixelFormat,
                    alphaFormat,
                    (IntPtr)ptr,
                    new PixelSize(tpc.Width, tpc.Height),
                    new Vector(96, 96),
                    stride);
            }
        }
    }
}
