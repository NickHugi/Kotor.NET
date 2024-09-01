using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Factories;
using Kotor.NET.Graphics.GPU;
using Silk.NET.OpenGL;

namespace Kotor.NET.Graphics.OpenGL.Factories;

public class TPCTextureFactory(GL _gl) : ITextureFactory
{
    public ITexture FromEmbeddedResource(string texture)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var textureStream = assembly.GetManifestResourceStream(texture)!;
        return FromStream(textureStream);
    }
    public ITexture FromFile(string texture)
    {
        using var textureStream = File.OpenRead(texture);
        return FromStream(textureStream);
    }
    public ITexture FromStream(Stream texture)
    {
        var vertexShaderSource = new StreamReader(texture).ReadToEnd();
        return FromSource();
    }
    public ITexture FromSource()
    {
        var textureID = _gl.GenTexture();

        _gl.BindTexture(GLEnum.Texture2D, textureID);

        // TODO
        //if (tpc.Format == TPCTextureFormat.DXT1)
        //{
        //    fixed (byte* buf = mipmap.Data)
        //        gl.CompressedTexImage2D(GLEnum.Texture2D, 0, InternalFormat.CompressedRgbS3TCDxt1Ext, (uint)tpc.Width, (uint)tpc.Height, 0, (uint)mipmap.Size, buf);
        //}
        //else if (tpc.Format == TPCTextureFormat.DXT5)
        //{
        //    fixed (byte* buf = mipmap.Data)
        //        gl.CompressedTexImage2D(GLEnum.Texture2D, 0, InternalFormat.CompressedRgbaS3TCDxt5Ext, (uint)tpc.Width, (uint)tpc.Height, 0, (uint)mipmap.Size, buf);
        //}
        //else if (tpc.Format == TPCTextureFormat.RGB)
        //{
        //    fixed (byte* buf = mipmap.Data)
        //        gl.TexImage2D(GLEnum.Texture2D, 0, InternalFormat.Rgb, (uint)mipmap.Width, (uint)mipmap.Height, 0, GLEnum.Rgb, PixelType.UnsignedByte, buf);
        //}
        //else if (tpc.Format == TPCTextureFormat.RGBA)
        //{
        //    fixed (byte* buf = mipmap.Data)
        //        gl.TexImage2D(GLEnum.Texture2D, 0, InternalFormat.Rgba, (uint)mipmap.Width, (uint)mipmap.Height, 0, GLEnum.Rgba, PixelType.UnsignedByte, buf);
        //}

        var textureWrapSFilter = (int)GLEnum.Repeat;
        _gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, textureWrapSFilter);
        var textureWrapTFilter = (int)GLEnum.Repeat;
        _gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, textureWrapTFilter);
        var textureMinFilter = (int)GLEnum.NearestMipmapLinear;
        _gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, textureMinFilter);
        var textureMagFilter = (int)GLEnum.Linear;
        _gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, textureMagFilter);

        _gl.GenerateMipmap(TextureTarget.Texture2D);

        return new GPU.Texture(_gl, textureID);
    }
}
