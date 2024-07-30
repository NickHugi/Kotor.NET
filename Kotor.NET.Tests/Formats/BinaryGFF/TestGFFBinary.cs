using System.Reflection.PortableExecutable;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryGFF;
using Kotor.NET.Resources.KotorGFF;
using Xunit;

namespace Kotor.NET.Tests.Formats.BinaryGFF;

public class TestGFFBinary
{
    public static readonly string File1Filepath = "Formats/BinaryGFF/file1.gff";

    private GFFBinary GetBinaryGFF(byte[] data)
    {
        return new GFFBinary(new MemoryStream(data));
    }
   
    [Fact]
    public void Test_ReadFile1()
    {
        var binaryGFF = GetBinaryGFF(File.ReadAllBytes(File1Filepath));

        Assert.Equal("GFF ", binaryGFF.FileHeader.FileType);
        Assert.Equal("V3.2", binaryGFF.FileHeader.FileVersion);
        Assert.Equal(3, binaryGFF.FileHeader.StructCount);
        Assert.Equal(4, binaryGFF.FileHeader.FieldCount);
        Assert.Equal(4, binaryGFF.FileHeader.LabelCount);
        Assert.Equal(8, binaryGFF.FileHeader.FieldDataLength);
        Assert.Equal(12, binaryGFF.FileHeader.FieldIndiciesLength);
        Assert.Equal(8, binaryGFF.FileHeader.ListIndiciesLength);

        Assert.Equal(3, binaryGFF.Structs.Count);
        Assert.Equal(4, binaryGFF.Fields.Count);
        Assert.Equal(4, binaryGFF.Labels.Count);
        Assert.Equal(8, binaryGFF.FieldData.Count());
        Assert.Equal(12, binaryGFF.FieldIndices.Count());
        Assert.Equal(8, binaryGFF.ListIndices.Count());
    }

    [Fact]
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
        binaryGFF.Write(stream);


        stream.Position = 0;
        var fileHeader = new GFFBinaryFileHeader(reader);
        Assert.Equal(3, binaryGFF.FileHeader.StructCount);
        Assert.Equal(4, binaryGFF.FileHeader.FieldCount);
        Assert.Equal(4, binaryGFF.FileHeader.LabelCount);

        stream.Position = binaryGFF.FileHeader.OffsetToStructs;
        var struct0 = new GFFBinaryStruct(reader);
        Assert.Equal(UInt32.MaxValue, struct0.ID);
        Assert.Equal<uint>(3, struct0.FieldCount);
        Assert.Equal(0, struct0.DataOrDataOffset);

        stream.Position = binaryGFF.FileHeader.OffsetToFields;
        var field0 = new GFFBinaryField(reader);
        Assert.Equal((int)GFFBinaryFieldType.String, field0.Type);
        Assert.Equal<uint>(0, field0.LabelIndex);

        stream.Position = binaryGFF.FileHeader.OffsetToLabels;
        var label0 = reader.ReadString(16);
        Assert.Equal("Field0", label0);
    }

    [Fact]
    public void Test_ParseFile1()
    {
        var binaryGFF = GetBinaryGFF(File.ReadAllBytes(File1Filepath));
        var gff = binaryGFF.Parse();

        Assert.Equal(3, gff.Root.FieldCount());
        Assert.Equal("text", gff.Root.GetString("Field0"));
        Assert.Equal<uint?>(5, gff.Root.GetList("List0")?.ElementAt(0).ID);
        Assert.Equal<byte?>(123, gff.Root.GetStruct("Struct0")?.GetUInt8("Field1"));
    }

    [Fact]
    public void Test_UnparseFile1()
    {
        var gff = new GFF();
        gff.Root.SetInt32("Int32", 123);
        gff.Root.SetInt64("Int64", (long)Int32.MaxValue + 1);
        var struct1 = gff.Root.SetStruct("Struct", 1);
        struct1.SetInt8("Int8", 12);
        var list = gff.Root.SetList("List");
        var struct2 = list.Add(2);
        var struct3 = list.Add(3);
        var struct4 = struct3.SetStruct("Struct", 4);
        struct4.SetResRef("ResRef", "abc");
        var struct5 = list.Add(5);
        struct5.SetString("String", "def");
        struct5.SetLocalisedString("LocalisedString1", new(13));
        struct5.SetLocalisedString("LocalisedString2", new(new List<LocalisedSubstring> { new(Language.German, Gender.Female, "Guten Tag") }));
        struct5.SetVector3("Vector3", new(1, 2, 3));
        struct5.SetVector4("Vector4", new(1, 2, 3, 4));
        struct5.SetBinary("Binary", new byte[] { 1, 2, 3 });

        var binaryGFF = new GFFBinary(gff);
        var reloadGFF = binaryGFF.Parse();

        Assert.Equal(reloadGFF.Root.GetInt32("Int32"), 123);
        Assert.Equal(reloadGFF.Root.GetInt64("Int64"), (long)Int32.MaxValue + 1);
        Assert.Equal<uint?>(reloadGFF.Root.GetStruct("Struct")?.ID, 1);
        Assert.Equal<sbyte?>(12, reloadGFF.Root.GetStruct("Struct")?.GetInt8("Int8"));
        Assert.Equal<uint?>(reloadGFF.Root.GetList("List")?.ElementAt(0)?.ID, 2);
        Assert.Equal<uint?>(reloadGFF.Root.GetList("List")?.ElementAt(1)?.ID, 3);
        Assert.Equal<uint?>(reloadGFF.Root.GetList("List")?.ElementAt(1)?.GetStruct("Struct")?.ID, 4);
        Assert.Equal("abc", reloadGFF.Root.GetList("List")?.ElementAt(1)?.GetStruct("Struct")?.GetResRef("ResRef")?.Get());
        Assert.Equal("def", reloadGFF.Root.GetList("List")?.ElementAt(2)?.GetString("String"));
        Assert.Equal(reloadGFF.Root.GetList("List")?.ElementAt(2)?.GetLocalisedString("LocalisedString1")?.StringRef, 13);
        Assert.Equal("Guten Tag", reloadGFF.Root.GetList("List")?.ElementAt(2)?.GetLocalisedString("LocalisedString2")?.GetSubstring(Language.German, Gender.Female));
        Assert.Equal(reloadGFF.Root.GetList("List")?.ElementAt(2)?.GetBinary("Binary"), new byte[] { 1, 2, 3 });
    }
}
