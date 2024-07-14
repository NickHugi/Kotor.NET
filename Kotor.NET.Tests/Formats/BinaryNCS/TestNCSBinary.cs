using System.Reflection.PortableExecutable;
using System.Text;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryNCS;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;

namespace Kotor.NET.Tests.Formats.BinaryNCS;

public class TestNCSBinary
{
    public static readonly string File1Filepath = "Formats/BinaryNCS/file1.NCS";

    [SetUp]
    public void Setup()
    {
        
    }

    private NCSBinary GetBinaryNCS(byte[] data)
    {
        return new NCSBinary(new MemoryStream(data));
    }
   
    [Test]
    public void Test_ReadFile1()
    {
        var binaryNCS = GetBinaryNCS(File.ReadAllBytes(File1Filepath));

        Assert.That(binaryNCS.FileHeader.FileType, Is.EqualTo("NCS "));
        Assert.That(binaryNCS.FileHeader.FileVersion, Is.EqualTo("V1.0"));
        Assert.That(binaryNCS.FileHeader.ProgramSize, Is.EqualTo(58));

        Assert.That(binaryNCS.Instructions.Count(), Is.EqualTo(8));

        var instruction0 = binaryNCS.Instructions.ElementAt(0);
        Assert.That(instruction0.ByteCode, Is.EqualTo(0x1E));
        Assert.That(instruction0.Type, Is.EqualTo(0x00));

        var instruction3 = binaryNCS.Instructions.ElementAt(3);
        Assert.That(instruction3.ByteCode, Is.EqualTo(0x04));
        Assert.That(instruction3.Type, Is.EqualTo(0x05));
        Assert.That(instruction3.Tail.Take(2).ToArray(), Is.EqualTo(new byte[] { 0x00, 0x09 }));
        Assert.That(Encoding.UTF8.GetString(instruction3.Tail.Skip(2).Take(9).ToArray()), Is.EqualTo("canderous"));
    }

    [Test]
    public void Test_RecalculateFile1()
    {
        var binaryNCS = GetBinaryNCS(File.ReadAllBytes(File1Filepath));
        binaryNCS.FileHeader.ProgramSize = Int32.MinValue;
        binaryNCS.Recalculate();

        Assert.That(binaryNCS.FileHeader.ProgramSize, Is.EqualTo(58));
    }

}
