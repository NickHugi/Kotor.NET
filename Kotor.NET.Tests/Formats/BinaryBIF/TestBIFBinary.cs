using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryBIF;

namespace Kotor.NET.Tests.Formats.BinaryBIF;

public class TestBIFBinary
{
    public static readonly string File1Filepath = "Formats/BinaryBIF/file1.bif";

    [SetUp]
    public void Setup()
    {
        
    }

    private BIFBinary GetBinaryBIF(byte[] data)
    {
        return new BIFBinary(new MemoryStream(data));
    }
   
    [Test]
    public void Test_ReadFile1()
    {
        var binaryBIF = GetBinaryBIF(File.ReadAllBytes(File1Filepath));

        Assert.That(binaryBIF.FileHeader.FileType, Is.EqualTo("BIFF"));
        Assert.That(binaryBIF.FileHeader.FileVersion, Is.EqualTo("V1  "));
        Assert.That(binaryBIF.FileHeader.ResourceCount, Is.EqualTo(1));
    }

    [Test]
    public void Test_RecalculateFile1()
    {
        var binaryBIF = GetBinaryBIF(File.ReadAllBytes(File1Filepath));
        binaryBIF.FileHeader.ResourceCount = Int32.MinValue;
        binaryBIF.FileHeader.OffsetToUnused = Int32.MinValue;
        binaryBIF.FileHeader.OffsetToResources = Int32.MinValue;
        binaryBIF.Recalculate();

        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binaryBIF.Write(stream);


        Assert.That(binaryBIF.FileHeader.ResourceCount, Is.EqualTo(1));

        stream.Position = binaryBIF.FileHeader.OffsetToResources;
        var resource0 = new BIFBinaryVariableResource(reader);
        Assert.That(resource0.ResourceID, Is.EqualTo(25165824));
        Assert.That(resource0.ResourceType, Is.EqualTo(ResourceType.JRL.ID));
    }
}
