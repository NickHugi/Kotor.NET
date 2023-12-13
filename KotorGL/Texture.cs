using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.FileFormats.KotorMDL;
using KotorDotNET.FileFormats.KotorTPC;
using OpenTK.Graphics.OpenGL4;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KotorGL
{
    public class Texture
    {
        public int ID { get; set; }

        public Texture(TPC tpc)
        {
            var mipmap = tpc.GetMipmap(0);

            ID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, ID);

            if (tpc.Format == TPCTextureFormat.DXT1)
            {
                GL.CompressedTexImage2D(TextureTarget.Texture2D, 0, InternalFormat.CompressedRgbS3tcDxt1Ext, tpc.Width, tpc.Height, 0, mipmap.Size, mipmap.Data);
            }
            else if (tpc.Format == TPCTextureFormat.DXT5)
            {
                GL.CompressedTexImage2D(TextureTarget.Texture2D, 0, InternalFormat.CompressedRgbaS3tcDxt5Ext, tpc.Width, tpc.Height, 0, mipmap.Size, mipmap.Data);
            }
            else if (tpc.Format == TPCTextureFormat.RGB)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, mipmap.Width, mipmap.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, mipmap.Data);
            }
            else if (tpc.Format == TPCTextureFormat.RGBA)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, mipmap.Width, mipmap.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, mipmap.Data);
            }

            var textureWrapSFilter = (int)All.Repeat;
            GL.TextureParameterI(ID, TextureParameterName.TextureWrapS, ref textureWrapSFilter);
            var textureWrapTFilter = (int)All.Repeat;
            GL.TextureParameterI(ID, TextureParameterName.TextureWrapT, ref textureWrapTFilter);
            var textureMinFilter = (int)All.NearestMipmapLinear;
            GL.TextureParameterI(ID, TextureParameterName.TextureMinFilter, ref textureMinFilter);
            var textureMagFilter = (int)All.Linear;
            GL.TextureParameterI(ID, TextureParameterName.TextureMagFilter, ref textureMagFilter);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, ID);
        }
    }
}
