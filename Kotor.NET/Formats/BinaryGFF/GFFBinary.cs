using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Helpers;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Formats.BinaryGFF;

public class GFFBinary
{
    public GFFBinaryFileHeader FileHeader { get; set; } = new();
    public List<GFFBinaryStruct> Structs { get; set; } = new();
    public List<GFFBinaryField> Fields { get; set; } = new();
    public List<string> Labels { get; set; } = new();
    public byte[] FieldData { get; set; } = new byte[0];
    public byte[] FieldIndices { get; set; } = new byte[0];
    public byte[] ListIndices { get; set; } = new byte[0];

    public GFFBinary()
    {
    }
    public GFFBinary(Stream stream)
    {
        var reader = new BinaryReader(stream);

        FileHeader = new GFFBinaryFileHeader(reader);

        reader.BaseStream.Position = FileHeader.OffsetToStructs;
        for (int i = 0; i < FileHeader.StructCount; i++)
        {
            Structs.Add(new GFFBinaryStruct(reader));
        }

        reader.BaseStream.Position = FileHeader.OffsetToFields;
        for (int i = 0; i < FileHeader.FieldCount; i++)
        {
            Fields.Add(new GFFBinaryField(reader));
        }

        reader.BaseStream.Position = FileHeader.OffsetToLabels;
        for (int i = 0; i < FileHeader.LabelCount; i++)
        {
            Labels.Add(reader.ReadString(16));
        }

        reader.BaseStream.Position = FileHeader.OffsetToFieldData;
        FieldData = reader.ReadBytes((int)FileHeader.FieldDataLength);

        reader.BaseStream.Position = FileHeader.OffsetToFieldIndices;
        FieldIndices = reader.ReadBytes((int)FileHeader.FieldIndiciesLength);

        reader.BaseStream.Position = FileHeader.OffsetToListIndicies;
        ListIndices = reader.ReadBytes((int)FileHeader.ListIndiciesLength);
    }
    public GFFBinary(GFF gff)
    {
        Unparse(gff);
    }

    public void Write(Stream stream)
    {
        var writer = new BinaryWriter(stream);

        FileHeader.Write(writer);

        writer.BaseStream.Position = FileHeader.OffsetToStructs;
        foreach (var @struct in Structs)
        {
            @struct.Write(writer);
        }

        writer.BaseStream.Position = FileHeader.OffsetToFields;
        foreach (var field in Fields)
        {
            field.Write(writer);
        }

        writer.BaseStream.Position = FileHeader.OffsetToLabels;
        foreach (var label in Labels)
        {
            writer.Write(label.Truncate(16).PadRight(16, '\0'), 0);
        }

        writer.BaseStream.Position = FileHeader.OffsetToFieldData;
        writer.Write(FieldData);

        writer.BaseStream.Position = FileHeader.OffsetToFieldIndices;
        writer.Write(FieldIndices);

        writer.BaseStream.Position = FileHeader.OffsetToListIndicies;
        writer.Write(ListIndices);
    }

    public void Recalculate()
    {
        FileHeader.FieldCount = Fields.Count;
        FileHeader.StructCount = Structs.Count;
        FileHeader.LabelCount = Labels.Count;
        FileHeader.FieldDataLength = FieldData.Count();
        FileHeader.FieldIndiciesLength = FieldIndices.Count();
        FileHeader.ListIndiciesLength = ListIndices.Count();

        var offset = GFFBinaryFileHeader.SIZE;
        FileHeader.OffsetToStructs = offset;

        offset += GFFBinaryStruct.SIZE * Structs.Count;
        FileHeader.OffsetToFields = offset;

        offset += GFFBinaryField.SIZE * Fields.Count;
        FileHeader.OffsetToLabels = offset;

        offset += 16 * Labels.Count;
        FileHeader.OffsetToFieldData = offset;

        offset += FieldData.Length;
        FileHeader.OffsetToFieldIndices = offset;

        offset += FieldIndices.Length;
        FileHeader.OffsetToListIndicies = offset;
    }

    public GFF Parse()
    {
        var gff = new GFF();

        gff.Root = ParseStruct(0);

        return gff;
    }
    private GFFStruct ParseStruct(int structIndex)
    {
        var gffStruct = new GFFStruct(Structs[structIndex].ID);

        if (Structs[structIndex].FieldCount == 1)
        {
            ParseField(gffStruct, Structs[structIndex].DataOrDataOffset);
        }
        else if (Structs[structIndex].FieldCount > 1)
        {
            for (int i = 0; i < Structs[structIndex].FieldCount; i++)
            {
                var fieldIndex = BitConverter.ToInt32(FieldIndices, Structs[structIndex].DataOrDataOffset + (i*4));
                ParseField(gffStruct, fieldIndex);
            }
        }

        return gffStruct;
    }
    private GFFList ParseList(int fieldIndicesOffset)
    {
        var list = new GFFList();

        var structCount = BitConverter.ToInt32(ListIndices, fieldIndicesOffset);

        for (int i = 0; i < structCount; i++)
        {
            var structIndex = BitConverter.ToInt32(ListIndices, fieldIndicesOffset + 4 + (i * 4));
            list.Add(ParseStruct(structIndex));
        }

        return list;
    }
    private void ParseField(GFFStruct parent, int fieldIndex)
    {
        var label = Labels[(int)Fields[fieldIndex].LabelIndex];
        var type = (GFFBinaryFieldType)Fields[fieldIndex].Type;
        var dataOrDataOffset = Fields[fieldIndex].DataOrDataOffset;
        var dataAsInt32 = BitConverter.ToInt32(Fields[fieldIndex].DataOrDataOffset);

        if (type == GFFBinaryFieldType.UInt8)
        {
            parent.SetUInt8(label, dataOrDataOffset[0]);
        }
        else if (type == GFFBinaryFieldType.Int8)
        {
            parent.SetInt8(label, unchecked((sbyte)dataOrDataOffset[0]));
        }
        else if (type == GFFBinaryFieldType.UInt16)
        {
            parent.SetUInt16(label, BitConverter.ToUInt16(dataOrDataOffset, 0));
        }
        else if (type == GFFBinaryFieldType.Int16)
        {
            parent.SetInt16(label, BitConverter.ToInt16(dataOrDataOffset, 0));
        }
        else if (type == GFFBinaryFieldType.UInt32)
        {
            parent.SetUInt32(label, BitConverter.ToUInt32(dataOrDataOffset, 0));
        }
        else if (type == GFFBinaryFieldType.Int32)
        {
            parent.SetInt32(label, BitConverter.ToInt32(dataOrDataOffset, 0));
        }
        else if (type == GFFBinaryFieldType.UInt64)
        {
            parent.SetUInt64(label, BitConverter.ToUInt64(FieldData, dataAsInt32));
        }
        else if (type == GFFBinaryFieldType.Int64)
        {
            parent.SetInt64(label, BitConverter.ToInt64(FieldData, dataAsInt32));
        }
        else if (type == GFFBinaryFieldType.Single)
        {
            parent.SetSingle(label, BitConverter.ToSingle(dataOrDataOffset, 0));
        }
        else if (type == GFFBinaryFieldType.Double)
        {
            parent.SetDouble(label, BitConverter.ToDouble(FieldData, dataAsInt32));
        }
        else if (type == GFFBinaryFieldType.String)
        {
            var length = BitConverter.ToInt32(FieldData, dataAsInt32);
            var text = Encoding.GetEncoding(1252).GetString(FieldData, dataAsInt32 + 4, length);
            parent.SetString(label, text);
        }
        else if (type == GFFBinaryFieldType.ResRef)
        {
            var length = FieldData[dataAsInt32];
            var text = Encoding.GetEncoding(1252).GetString(FieldData, dataAsInt32 + 1, length);
            parent.SetResRef(label, text);
        }
        else if (type == GFFBinaryFieldType.LocalisedString)
        {
            var totalSize = BitConverter.ToInt32(FieldData, dataAsInt32);
            var stringRef = BitConverter.ToInt32(FieldData, dataAsInt32 + 4);
            var stringCount = BitConverter.ToInt32(FieldData, dataAsInt32 + 8);

            var locstring = new LocalisedString();
            locstring.StringRef = stringRef;

            int offset = dataAsInt32 + 12;
            for (int i = 0; i < stringCount; i++)
            {
                var stringID = BitConverter.ToInt32(FieldData, offset);
                var stringLength = BitConverter.ToInt32(FieldData, offset + 4);
                var text = KEncoding.GetWindows1252().GetString(FieldData, offset + 8, stringLength);

                var language = (Language)(stringID >> 1);
                var gender = (Gender)(stringID % 2);
                locstring.SetSubstring(language, gender, text);

                offset += 8 + stringLength;
            }

            parent.SetLocalisedString(label, locstring);
        }
        else if (type == GFFBinaryFieldType.Binary)
        {
            var length = BitConverter.ToInt32(FieldData, dataAsInt32);
            var data = FieldData.Skip(dataAsInt32 + 4).Take(length).ToArray();
            parent.SetBinary(label, data);
        }
        else if (type == GFFBinaryFieldType.Struct)
        {
            var child = ParseStruct(dataAsInt32);
            parent.SetStruct(label, child);
        }
        else if (type == GFFBinaryFieldType.List)
        {
            var list = ParseList(dataAsInt32);
            parent.SetList(label, list);
        }
        else if (type == GFFBinaryFieldType.Vector3)
        {
            var x = BitConverter.ToSingle(FieldData, dataAsInt32);
            var y = BitConverter.ToSingle(FieldData, dataAsInt32 + 4);
            var z = BitConverter.ToSingle(FieldData, dataAsInt32 + 8);
            parent.SetVector3(label, new(x, y, z));
        }
        else if (type == GFFBinaryFieldType.Vector3)
        {
            var x = BitConverter.ToSingle(FieldData, dataAsInt32);
            var y = BitConverter.ToSingle(FieldData, dataAsInt32 + 4);
            var z = BitConverter.ToSingle(FieldData, dataAsInt32 + 8);
            var w = BitConverter.ToSingle(FieldData, dataAsInt32 + 12);
            parent.SetVector4(label, new(x, y, z, w));
        }
    }

    public void Unparse(GFF gff)
    {
        FileHeader.FileType = gff.Type.ToString().ToUpper().PadRight(4, ' ');

        UnparseStruct(gff.Root);

        Recalculate();
    }
    private int UnparseStruct(GFFStruct gffStruct)
    {
        var structIndex = Structs.Count();

        Structs.Add(new()
        {
            ID = gffStruct.ID,
            FieldCount = (uint)gffStruct.FieldCount()
        });

        var fieldIndices = new List<int>();
        foreach (var item in gffStruct._data)
        {
            var index = UnparseField(gffStruct, item.Key);
            fieldIndices.Add(index);
        }

        if (gffStruct.FieldCount() == 1)
        {
            Structs[structIndex].DataOrDataOffset = fieldIndices.Last();
        }
        else if (gffStruct.FieldCount() > 1)
        {
            Structs[structIndex].DataOrDataOffset = FieldIndices.Count();
            FieldIndices = FieldIndices.Concat(fieldIndices.SelectMany(x => BitConverter.GetBytes(x))).ToArray();
        }

        return structIndex;
    }
    private int UnparseField(GFFStruct gffStruct, string fieldName)
    {
        var labelIndex = Labels.Count();
        Labels.Add(fieldName);

        GFFBinaryFieldType fieldType;
        byte[] dataOrDataOffset = new byte[0];

        if (gffStruct._data[fieldName] is byte uint8)
        {
            fieldType = GFFBinaryFieldType.UInt8;
            dataOrDataOffset = BitConverter.GetBytes((uint)uint8);
        }
        else if (gffStruct._data[fieldName] is sbyte int8)
        {
            fieldType = GFFBinaryFieldType.Int8;
            dataOrDataOffset = BitConverter.GetBytes((int)int8);
        }
        else if (gffStruct._data[fieldName] is ushort uint16)
        {
            fieldType = GFFBinaryFieldType.UInt16;
            dataOrDataOffset = BitConverter.GetBytes((uint)uint16);
        }
        else if (gffStruct._data[fieldName] is short int16)
        {
            fieldType = GFFBinaryFieldType.Int16;
            dataOrDataOffset = BitConverter.GetBytes((int)int16);
        }
        else if (gffStruct._data[fieldName] is uint uint32)
        {
            fieldType = GFFBinaryFieldType.UInt32;
            dataOrDataOffset = BitConverter.GetBytes((uint)uint32);
        }
        else if (gffStruct._data[fieldName] is int int32)
        {
            fieldType = GFFBinaryFieldType.Int32;
            dataOrDataOffset = BitConverter.GetBytes((int)int32);
        }
        else if (gffStruct._data[fieldName] is ulong uint64)
        {
            fieldType = GFFBinaryFieldType.UInt64;
            var offsetToFieldData = FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            FieldData = FieldData.Concat(BitConverter.GetBytes(uint64)).ToArray();
        }
        else if (gffStruct._data[fieldName] is long int64)
        {
            fieldType = GFFBinaryFieldType.Int64;
            var offsetToFieldData = FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            FieldData = FieldData.Concat(BitConverter.GetBytes(int64)).ToArray();
        }
        else if (gffStruct._data[fieldName] is float singlef)
        {
            fieldType = GFFBinaryFieldType.Single;
            dataOrDataOffset = BitConverter.GetBytes(singlef);
        }
        else if (gffStruct._data[fieldName] is double doublef)
        {
            fieldType = GFFBinaryFieldType.Double;
            var offsetToFieldData = FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            FieldData = FieldData.Concat(BitConverter.GetBytes(doublef)).ToArray();
        }
        else if (gffStruct._data[fieldName] is string text)
        {
            fieldType = GFFBinaryFieldType.String;
            var offsetToFieldData = FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            FieldData = FieldData.Concat(BitConverter.GetBytes(text.Length)).Concat(KEncoding.GetWindows1252().GetBytes(text)).ToArray();
        }
        else if (gffStruct._data[fieldName] is ResRef resref)
        {
            fieldType = GFFBinaryFieldType.ResRef;
            var offsetToFieldData = FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            FieldData = FieldData.Concat(new byte[] { (byte)resref.Length() }).Concat(KEncoding.GetWindows1252().GetBytes(resref.Get())).ToArray();
        }
        else if (gffStruct._data[fieldName] is LocalisedString locstring)
        {
            fieldType = GFFBinaryFieldType.LocalisedString;
            var offsetToFieldData = FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            var totalSize = 12 + locstring.AllSubstrings().Sum(x => x.Text.Length + 8);
            FieldData = FieldData
                .Concat(BitConverter.GetBytes(totalSize))
                .Concat(BitConverter.GetBytes(locstring.StringRef))
                .Concat(BitConverter.GetBytes(locstring.Count()))
                .ToArray();
            locstring.AllSubstrings().ToList().ForEach(x => FieldData = FieldData
                .Concat(BitConverter.GetBytes(x.StringID))
                .Concat(BitConverter.GetBytes(x.Text.Length))
                .Concat(KEncoding.GetWindows1252().GetBytes(x.Text))
                .ToArray());
        }
        else if (gffStruct._data[fieldName] is byte[] data)
        {
            fieldType = GFFBinaryFieldType.Binary;
            var offsetToFieldData = FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            FieldData = FieldData.Concat(BitConverter.GetBytes(data.Length)).Concat(data).ToArray();
        }
        else if (gffStruct._data[fieldName] is GFFStruct child)
        {
            fieldType = GFFBinaryFieldType.Struct;
            var structIndex = UnparseStruct(child);
            dataOrDataOffset = BitConverter.GetBytes(structIndex);
        }
        else if (gffStruct._data[fieldName] is GFFList list)
        {
            fieldType = GFFBinaryFieldType.List;

            var indicies = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                indicies.Add(UnparseStruct(list.ElementAt(i)));
            }

            dataOrDataOffset = BitConverter.GetBytes(ListIndices.Count());
            ListIndices = ListIndices.Concat(BitConverter.GetBytes(indicies.Count())).ToArray();
            foreach (var index in indicies)
            {
                ListIndices = ListIndices.Concat(BitConverter.GetBytes(index)).ToArray();
            }
        }
        else if (gffStruct._data[fieldName] is Vector3 vector3)
        {
            fieldType = GFFBinaryFieldType.Vector3;
            var offsetToFieldData = FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            FieldData = FieldData
                .Concat(BitConverter.GetBytes(vector3.X))
                .Concat(BitConverter.GetBytes(vector3.Y))
                .Concat(BitConverter.GetBytes(vector3.Z))
                .ToArray();
        }
        else if (gffStruct._data[fieldName] is Vector4 vector4)
        {
            fieldType = GFFBinaryFieldType.Vector4;
            var offsetToFieldData = FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            FieldData = FieldData
                .Concat(BitConverter.GetBytes(vector4.X))
                .Concat(BitConverter.GetBytes(vector4.Y))
                .Concat(BitConverter.GetBytes(vector4.Z))
                .Concat(BitConverter.GetBytes(vector4.W))
                .ToArray();
        }
        else
            throw new Exception("Illegal type got assigned to a GFF field somehow?");

        var fieldIndex = Fields.Count();
        Fields.Add(new()
        {
            LabelIndex = (uint)labelIndex,
            Type = (int)fieldType,
            DataOrDataOffset = dataOrDataOffset,
        });

        return fieldIndex;
    }
}
