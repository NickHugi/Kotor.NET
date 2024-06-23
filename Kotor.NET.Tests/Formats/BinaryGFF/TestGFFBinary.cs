using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryGFF;

namespace Kotor.NET.Tests.Formats.BinaryGFF;

public class TestGFFBinary
{
    public static readonly string File1Filepath = "Formats/BinaryGFF/file1.gff";

    [SetUp]
    public void Setup()
    {
        
    }

    private GFFBinary GetBinaryGFF(byte[] data)
    {
        var reader = new BinaryReader(new MemoryStream(data));
        return new GFFBinary(reader);
    }
   
    [Test]
    public void Test_ReadFile1()
    {
        var binaryGFF = GetBinaryGFF(File.ReadAllBytes(File1Filepath));

        Assert.That(binaryGFF.FileHeader.FileType, Is.EqualTo("GFF "));
        Assert.That(binaryGFF.FileHeader.FileVersion, Is.EqualTo("V3.2"));
        Assert.That(binaryGFF.FileHeader.StructCount, Is.EqualTo(3));
        Assert.That(binaryGFF.FileHeader.FieldCount, Is.EqualTo(4));
        Assert.That(binaryGFF.FileHeader.LabelCount, Is.EqualTo(4));
        Assert.That(binaryGFF.FileHeader.FieldDataLength, Is.EqualTo(8));
        Assert.That(binaryGFF.FileHeader.FieldIndiciesLength, Is.EqualTo(12));
        Assert.That(binaryGFF.FileHeader.ListIndiciesLength, Is.EqualTo(8));

        Assert.That(binaryGFF.Structs.Count, Is.EqualTo(3));
        Assert.That(binaryGFF.Fields.Count, Is.EqualTo(4));
        Assert.That(binaryGFF.Labels.Count, Is.EqualTo(4));
        Assert.That(binaryGFF.FieldData.Count, Is.EqualTo(8));
        Assert.That(binaryGFF.FieldIndices.Count, Is.EqualTo(12));
        Assert.That(binaryGFF.ListIndices.Count, Is.EqualTo(8));
    }

    [Test]
    public void Test_RecalculateFile1()
    {
        var binaryGFF = GetBinaryGFF(File.ReadAllBytes(File1Filepath));
        binaryGFF.FileHeader.StructCount = Int32.MinValue;
        binaryGFF.FileHeader.FieldCount = Int32.MinValue;
        binaryGFF.FileHeader.LabelCount = Int32.MinValue;
        binaryGFF.FileHeader.OffsetToStructs = Int32.MinValue;
        binaryGFF.FileHeader.OffsetToFields = Int32.MinValue;
        binaryGFF.FileHeader.OffsetToLabels = Int32.MinValue;
        binaryGFF.FileHeader.OffsetToFieldData = Int32.MinValue;
        binaryGFF.FileHeader.OffsetToFieldIndices = Int32.MinValue;
        binaryGFF.FileHeader.OffsetToListIndicies = Int32.MinValue;
        binaryGFF.Recalculate();

        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binaryGFF.Write(new BinaryWriter(stream));


        Assert.That(binaryGFF.FileHeader.StructCount, Is.EqualTo(3));
        Assert.That(binaryGFF.FileHeader.FieldCount, Is.EqualTo(4));
        Assert.That(binaryGFF.FileHeader.LabelCount, Is.EqualTo(4));

        stream.Position = binaryGFF.FileHeader.OffsetToStructs;
        var struct0 = new GFFBinaryStruct(reader);
        Assert.That(struct0.ID, Is.EqualTo(UInt32.MaxValue));
        Assert.That(struct0.FieldCount, Is.EqualTo(3));
        Assert.That(struct0.DataOrDataOffset, Is.EqualTo(0));

        stream.Position = binaryGFF.FileHeader.OffsetToFields;
        var field0 = new GFFBinaryField(reader);
        Assert.That(field0.Type, Is.EqualTo((int)GFFBinaryFieldType.String));
        Assert.That(field0.LabelIndex, Is.EqualTo(0));

        stream.Position = binaryGFF.FileHeader.OffsetToLabels;
        var label0 = reader.ReadString(16);
        Assert.That(label0, Is.EqualTo("Field0"));
    }
}
