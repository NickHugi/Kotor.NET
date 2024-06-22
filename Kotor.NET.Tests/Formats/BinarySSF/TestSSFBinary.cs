using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinarySSF;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;

namespace Kotor.NET.Tests.Formats.BinarySSF;

public class TestSSFBinary
{
    public static readonly string File1Filepath = "Formats/BinarySSF/file1.ssf";

    [SetUp]
    public void Setup()
    {
        
    }

    private SSFBinary GetBinarySSF(byte[] data)
    {
        var reader = new BinaryReader(new MemoryStream(data));
        return new SSFBinary(reader);
    }
   
    [Test]
    public void Test_ReadFile1()
    {
        var binarySSF = GetBinarySSF(File.ReadAllBytes(File1Filepath));

        Assert.That(binarySSF.FileHeader.FileType, Is.EqualTo("SSF "));
        Assert.That(binarySSF.FileHeader.FileVersion, Is.EqualTo("V1.1"));

        Assert.That(binarySSF.SoundList.Sounds.Length, Is.EqualTo(40), "Key frame list did not build correctly.");

        for (int i = 0; i < 28; i ++)
        {
            var strref = 123075 - i;
            Assert.That(binarySSF.SoundList.Sounds[i], Is.EqualTo(strref));
        }
    }

    [Test]
    public void Test_RecalculateFile1()
    {
        var binarySSF = GetBinarySSF(File.ReadAllBytes(File1Filepath));
        binarySSF.FileHeader.OffsetToSounds = Int32.MinValue;
        binarySSF.SoundList.Sounds = new uint[0];
        binarySSF.Recalculate();

        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binarySSF.Write(new BinaryWriter(stream));


        Assert.That(binarySSF.SoundList.Sounds.Length, Is.EqualTo(40));

        stream.Position = binarySSF.FileHeader.OffsetToSounds;
        var soundList = new SSFBinarySoundList(reader);
        Assert.That(soundList.Sounds[0], Is.EqualTo(4294967295));
    }

}
