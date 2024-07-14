using System.Reflection.PortableExecutable;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryGFF;
using Kotor.NET.Resources.KotorGFF;

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
        return new GFFBinary(new MemoryStream(data));
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
        binaryGFF.Write(stream);


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

    [Test]
    public void Test_ParseFile1()
    {
        var binaryGFF = GetBinaryGFF(File.ReadAllBytes(File1Filepath));
        var gff = binaryGFF.Parse();

        Assert.That(gff.Root.FieldCount(), Is.EqualTo(3));
        Assert.That(gff.Root.GetString("Field0"), Is.EqualTo("text"));
        Assert.That(gff.Root.GetList("List0")?.ElementAt(0).ID, Is.EqualTo(5));
        Assert.That(gff.Root.GetStruct("Struct0")?.GetUInt8("Field1"), Is.EqualTo(123));
    }

    [Test]
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

        Assert.That(reloadGFF.Root.GetInt32("Int32"), Is.EqualTo(123));
        Assert.That(reloadGFF.Root.GetInt64("Int64"), Is.EqualTo((long)Int32.MaxValue + 1));
        Assert.That(reloadGFF.Root.GetStruct("Struct")?.ID, Is.EqualTo(1));
        Assert.That(reloadGFF.Root.GetStruct("Struct")?.GetInt8("Int8"), Is.EqualTo(12));
        Assert.That(reloadGFF.Root.GetList("List")?.ElementAt(0)?.ID, Is.EqualTo(2));
        Assert.That(reloadGFF.Root.GetList("List")?.ElementAt(1)?.ID, Is.EqualTo(3));
        Assert.That(reloadGFF.Root.GetList("List")?.ElementAt(1)?.GetStruct("Struct")?.ID, Is.EqualTo(4));
        Assert.That(reloadGFF.Root.GetList("List")?.ElementAt(1)?.GetStruct("Struct")?.GetResRef("ResRef")?.Get(), Is.EqualTo("abc"));
        Assert.That(reloadGFF.Root.GetList("List")?.ElementAt(2)?.GetString("String"), Is.EqualTo("def"));
        Assert.That(reloadGFF.Root.GetList("List")?.ElementAt(2)?.GetLocalisedString("LocalisedString1")?.StringRef, Is.EqualTo(13));
        Assert.That(reloadGFF.Root.GetList("List")?.ElementAt(2)?.GetLocalisedString("LocalisedString2")?.GetSubstring(Language.German, Gender.Female), Is.EqualTo("Guten Tag"));
        Assert.That(reloadGFF.Root.GetList("List")?.ElementAt(2)?.GetBinary("Binary"), Is.EqualTo(new byte[] { 1, 2, 3 }));
    }
}
