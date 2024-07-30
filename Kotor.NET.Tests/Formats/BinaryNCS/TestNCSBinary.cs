using System.Reflection.PortableExecutable;
using System.Text;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryNCS;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Xunit;

namespace Kotor.NET.Tests.Formats.BinaryNCS;

public class TestNCSBinary
{
    public static readonly string File1Filepath = "Formats/BinaryNCS/file1.NCS";

    private NCSBinary GetBinaryNCS(byte[] data)
    {
        return new NCSBinary(new MemoryStream(data));
    }
   
    [Fact]
    public void Test_ReadFile1()
    {
        var binaryNCS = GetBinaryNCS(File.ReadAllBytes(File1Filepath));

        Assert.Equal("NCS ", binaryNCS.FileHeader.FileType);
        Assert.Equal("V1.0", binaryNCS.FileHeader.FileVersion);
        Assert.Equal(58, binaryNCS.FileHeader.ProgramSize);

        Assert.Equal(8, binaryNCS.Instructions.Count());

        var instruction0 = binaryNCS.Instructions.ElementAt(0);
        Assert.Equal(0x1E, instruction0.ByteCode);
        Assert.Equal(0x00, instruction0.Type);

        var instruction3 = binaryNCS.Instructions.ElementAt(3);
        Assert.Equal(0x04, instruction3.ByteCode);
        Assert.Equal(0x05, instruction3.Type);
        Assert.Equal(instruction3.Tail.Take(2).ToArray(), new byte[] { 0x00, 0x09 });
        Assert.Equal("canderous", Encoding.UTF8.GetString(instruction3.Tail.Skip(2).Take(9).ToArray()));
    }

    [Fact]
    public void Test_RecalculateFile1()
    {
        var binaryNCS = GetBinaryNCS(File.ReadAllBytes(File1Filepath));
        binaryNCS.FileHeader.ProgramSize = Int32.MinValue;
        binaryNCS.Recalculate();

        Assert.Equal(58, binaryNCS.FileHeader.ProgramSize);
    }

}
