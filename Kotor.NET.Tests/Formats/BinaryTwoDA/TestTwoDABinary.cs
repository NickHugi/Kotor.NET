using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.Binary2DA;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Xunit;

namespace Kotor.NET.Tests.Formats.BinaryTwoDA;

public class TestTwoDABinary
{
    public static readonly string File1Filepath = "Formats/BinaryTwoDA/file1.2da";

    private TwoDABinary GetBinaryTwoDA(byte[] data)
    {
        return new TwoDABinary(new MemoryStream(data));
    }
   
    [Fact]
    public void Test_ReadFile1()
    {
        var binaryTwoDA = GetBinaryTwoDA(File.ReadAllBytes(File1Filepath));

        Assert.Equal("2DA ", binaryTwoDA.FileHeader.FileType);
        Assert.Equal("V2.b", binaryTwoDA.FileHeader.FileVersion);

        Assert.Equal(3, binaryTwoDA.ColumnHeaders.Count());
        Assert.Equal(9, binaryTwoDA.CellValues.Count());

        Assert.Equal("col3", binaryTwoDA.ColumnHeaders[0]);
        Assert.Equal("col2", binaryTwoDA.ColumnHeaders[1]);
        Assert.Equal("col1", binaryTwoDA.ColumnHeaders[2]);

        Assert.Equal("10", binaryTwoDA.RowHeaders[0]);
        Assert.Equal("1", binaryTwoDA.RowHeaders[1]);
        Assert.Equal("2", binaryTwoDA.RowHeaders[2]);

        Assert.Equal("ghi", binaryTwoDA.CellValues[0]);
        Assert.Equal("def", binaryTwoDA.CellValues[1]);
        Assert.Equal("abc", binaryTwoDA.CellValues[2]);
        Assert.Equal("123", binaryTwoDA.CellValues[3]);
        Assert.Equal("ghi", binaryTwoDA.CellValues[4]);
        Assert.Equal("def", binaryTwoDA.CellValues[5]);
        Assert.Equal("abc", binaryTwoDA.CellValues[6]);
        Assert.Equal("", binaryTwoDA.CellValues[7]);
        Assert.Equal("123", binaryTwoDA.CellValues[8]);
    }
}
