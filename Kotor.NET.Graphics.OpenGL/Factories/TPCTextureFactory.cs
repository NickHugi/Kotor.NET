using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinaryTPC.Serialisation;
using Kotor.NET.Graphics.Factories;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Resources.KotorTPC;
using Kotor.NET.Resources.KotorTPC.TextureFormats;
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
    public unsafe ITexture FromStream(Stream stream)
    {
        var tpc = new TPCBinaryDeserializer(new Formats.BinaryTPC.TPCBinary(stream)).Deserialize();
        var textureID = _gl.GenTexture();

        _gl.BindTexture(GLEnum.Texture2D, textureID);

        _gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        _gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        _gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        _gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

        // TODO
        if (tpc.TextureFormat == TPCTextureFormat.DXT1)
        {
            var data = tpc.GetData(0, 0);
            fixed (byte* buf = data)
                _gl.CompressedTexImage2D(GLEnum.Texture2D, 0, InternalFormat.CompressedRgbS3TCDxt1Ext, (uint)tpc.Width, (uint)tpc.Height, 0, (uint)data.Length, buf);
        }
        else if (tpc.TextureFormat == TPCTextureFormat.DXT5)
        {
            var data = tpc.GetData(0, 0);
            fixed (byte* buf = data)
                _gl.CompressedTexImage2D(GLEnum.Texture2D, 0, InternalFormat.CompressedRgbaS3TCDxt5Ext, (uint)tpc.Width, (uint)tpc.Height, 0, (uint)data.Length, buf);
        }
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

        _gl.GenerateMipmap(TextureTarget.Texture2D);

        return new GPU.Texture(_gl, textureID);
    }
    public unsafe ITexture FromPlaceholder()
    {
        var textureID = _gl.GenTexture();

        _gl.BindTexture(GLEnum.Texture2D, textureID);

        _gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        _gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        _gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        _gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

        var data = Enumerable.Range(0, 32 * 32).SelectMany(x => new byte[] { 255, 0, 255, 255 }).ToArray();

        fixed (byte* buf = data)
            _gl.TexImage2D(GLEnum.Texture2D, 0, InternalFormat.Rgba, 32, 32, 0, GLEnum.Rgba, PixelType.UnsignedByte, buf);

        return new GPU.Texture(_gl, textureID);
    }
}
