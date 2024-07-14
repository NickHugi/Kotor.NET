using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.Binary2DA;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;

namespace Kotor.NET.Tests.Formats.BinaryTwoDA;

public class TestTwoDABinary
{
    public static readonly string File1Filepath = "Formats/BinaryTwoDA/file1.2da";

    [SetUp]
    public void Setup()
    {
        
    }

    private TwoDABinary GetBinaryTwoDA(byte[] data)
    {
        return new TwoDABinary(new MemoryStream(data));
    }
   
    [Test]
    public void Test_ReadFile1()
    {
        var binaryTwoDA = GetBinaryTwoDA(File.ReadAllBytes(File1Filepath));

        Assert.That(binaryTwoDA.FileHeader.FileType, Is.EqualTo("2DA "));
        Assert.That(binaryTwoDA.FileHeader.FileVersion, Is.EqualTo("V2.b"));

        Assert.That(binaryTwoDA.ColumnHeaders.Count(), Is.EqualTo(3));
        Assert.That(binaryTwoDA.CellValues.Count(), Is.EqualTo(9));

        Assert.That(binaryTwoDA.ColumnHeaders[0], Is.EqualTo("col3"));
        Assert.That(binaryTwoDA.ColumnHeaders[1], Is.EqualTo("col2"));
        Assert.That(binaryTwoDA.ColumnHeaders[2], Is.EqualTo("col1"));

        Assert.That(binaryTwoDA.RowHeaders[0], Is.EqualTo("10"));
        Assert.That(binaryTwoDA.RowHeaders[1], Is.EqualTo("1"));
        Assert.That(binaryTwoDA.RowHeaders[2], Is.EqualTo("2"));

        Assert.That(binaryTwoDA.CellValues[0], Is.EqualTo("ghi"));
        Assert.That(binaryTwoDA.CellValues[1], Is.EqualTo("def"));
        Assert.That(binaryTwoDA.CellValues[2], Is.EqualTo("abc"));
        Assert.That(binaryTwoDA.CellValues[3], Is.EqualTo("123"));
        Assert.That(binaryTwoDA.CellValues[4], Is.EqualTo("ghi"));
        Assert.That(binaryTwoDA.CellValues[5], Is.EqualTo("def"));
        Assert.That(binaryTwoDA.CellValues[6], Is.EqualTo("abc"));
        Assert.That(binaryTwoDA.CellValues[7], Is.EqualTo(""));
        Assert.That(binaryTwoDA.CellValues[8], Is.EqualTo("123"));
    }
}
