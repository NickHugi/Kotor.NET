using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.FileFormats.KotorMDL;
using KotorDotNET.FileFormats.KotorTPC;

namespace KotorGL
{
    public class Texture
    {
        public int ID { get; set; }

        public Texture(TPC tpc)
        {
            var mipmap = tpc.GetMipmap(0);

            //ID = GL.GenTexture();
            //GL.BindTexture(All.Texture2D, ID);

            //if (tpc.Format == TPCTextureFormat.DXT1)
            //{
            //    GL.CompressedTexImage2D(All.Texture2D, 0, All.CompressedRgbS3tcDxt1Ext, tpc.Width, tpc.Height, 0, mipmap.Size, mipmap.Data);
            //}
            //else if (tpc.Format == TPCTextureFormat.DXT5)
            //{
            //    GL.CompressedTexImage2D(All.Texture2D, 0, All.CompressedRgbaS3tcDxt5Ext, tpc.Width, tpc.Height, 0, mipmap.Size, mipmap.Data);
            //}
            //else if (tpc.Format == TPCTextureFormat.RGB)
            //{
            //    GL.TexImage2D(All.Texture2D, 0, All.Rgb, mipmap.Width, mipmap.Height, 0, All.Rgb, All.UnsignedByte, mipmap.Data);
            //}
            //else if (tpc.Format == TPCTextureFormat.RGBA)
            //{
            //    GL.TexImage2D(All.Texture2D, 0, All.Rgba, mipmap.Width, mipmap.Height, 0, All.Rgba, All.UnsignedByte, mipmap.Data);
            //}

            //var textureWrapSFilter = (int)All.Repeat;
            //GL.TexturePar(ID, TextureParameterName.TextureWrapS, ref textureWrapSFilter);
            //var textureWrapTFilter = (int)All.Repeat;
            //GL.TextureParameterI(ID, TextureParameterName.TextureWrapT, ref textureWrapTFilter);
            //var textureMinFilter = (int)All.NearestMipmapLinear;
            //GL.TextureParameterI(ID, TextureParameterName.TextureMinFilter, ref textureMinFilter);
            //var textureMagFilter = (int)All.Linear;
            //GL.TextureParameterI(ID, TextureParameterName.TextureMagFilter, ref textureMagFilter);

            //GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use()
        {
            //GL.BindTexture(All.Texture2D, ID);
        }
    }
}
