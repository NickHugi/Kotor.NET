using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinarySSF;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Xunit;

namespace Kotor.NET.Tests.Formats.BinarySSF;

public class TestSSFBinary
{
    public static readonly string File1Filepath = "Formats/BinarySSF/file1.ssf";

    private SSFBinary GetBinarySSF(byte[] data)
    {
        return new SSFBinary(new MemoryStream(data));
    }
   
    [Fact]
    public void Test_ReadFile1()
    {
        var binarySSF = GetBinarySSF(File.ReadAllBytes(File1Filepath));

        Assert.Equal("SSF ", binarySSF.FileHeader.FileType);
        Assert.Equal("V1.1", binarySSF.FileHeader.FileVersion);

        Assert.Equal(40, binarySSF.SoundList.Sounds.Length);

        for (int i = 0; i < 28; i ++)
        {
            var strref = 123075 - i;
            Assert.Equal((int)binarySSF.SoundList.Sounds.ElementAt(i), strref);
        }
    }

    [Fact]
    public void Test_RecalculateFile1()
    {
        var binarySSF = GetBinarySSF(File.ReadAllBytes(File1Filepath));
        binarySSF.FileHeader.OffsetToSounds = Int32.MinValue;
        binarySSF.SoundList.Sounds = new uint[0];
        binarySSF.Recalculate();

        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binarySSF.Write(stream);


        Assert.Equal(40, binarySSF.SoundList.Sounds.Length);

        stream.Position = binarySSF.FileHeader.OffsetToSounds;
        var soundList = new SSFBinarySoundList(reader);
        Assert.Equal(4294967295, soundList.Sounds[0]);
    }

}
