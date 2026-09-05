using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Patcher.For2DA;
using Kotor.NET.Resources.Kotor2DA;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF;

public class PatchGFF : IPatch
{
    public required string ResRef { get; init; }
    public required ResourceType ResourceType { get; init; }
    public required ILocateContainer TakeFrom { get; set; }
    public required ILocateContainer SaveTo { get; set; }
    public required IFileOperation FileOperation { get; set; }
    public ICollection<IGFFModifier> Modifiers { get; set; } = [];

    public void Apply(Installation installation, PatcherMemory memory)
    {
        var data = FileOperation.Read(installation, TakeFrom, ResRef, ResourceType);
        var gff = (data is null) ? (new GFF()) : GFF.FromBytes(data);
        
        Modifiers.ToList().ForEach(x => x.Apply(gff, new RootNode() { Struct = gff.Root }, installation, memory));

        data = GFF.ToBytes(gff);
        FileOperation.Write(installation, SaveTo, ResRef, ResourceType, data);
    }
}
public class EditCreature : PatchGFF
{
    public EditCreature()
    {
        //TakeFrom = new HardcodedLocateResource();
        //SaveTo = new HardcodedLocateResource();
    }
}


public interface IValue<T>
{
    public T Get(GFF gff, Installation installation, PatcherMemory memory);
}
public class ConstantValue<T> : IValue<T>
{
    public required T Value { get; set; }

    public T Get(GFF gff, Installation installation, PatcherMemory memory)
    {
        return Value;
    }
}
public class TokenValue<T> : IValue<T>
{
    public required string Token { get; set; }

    public T Get(GFF gff, Installation installation, PatcherMemory memory)
    {
        return memory.Get<T>(Token);
    }
}
public class TwoDARowIndexValue<T> : IValue<T>  
{
    public required string ResRef { get; set; }
    public required string SearchColumn { get; set; }
    public required string SearchForCell { get; set; }

    public T Get(GFF gff, Installation installation, PatcherMemory memory)
    {
        var twoda = installation.Get2DA(ResRef);

        var row = twoda.GetRows().Single(x => x.GetCell(SearchColumn).AsString() == SearchForCell);
        var index = row.Index;

        return typeof(T) switch
        {
            Type x when x == typeof(byte) => (T)(object)index,
            Type x when x == typeof(sbyte) => (T)(object)index,
            Type x when x == typeof(ushort) => (T)(object)index,
            Type x when x == typeof(short) => (T)(object)index,
            Type x when x == typeof(uint) => (T)(object)index,
            Type x when x == typeof(int) => (T)(object)index,
            Type x when x == typeof(ulong) => (T)(object)index,
            Type x when x == typeof(long) => (T)(object)index,
            Type x when x == typeof(string) => (T)(object)index.ToString(),
            _ => throw new Exception() // TODO
        };
    }
}

public interface IFieldLocator
{
    public INode Locate(GFF gff, INode cursor, Installation installation, PatcherMemory memory);
}
//public class ParentFieldLocator : IFieldLocator
//{
//    public Field Locate(GFF gff, GFFStruct parent, Installation installation, PatcherMemory memory)
//    {
//        new Field()
//        {
//            Struct = parent,
//        };
//    }
//}
public class ByPathFieldLocator : IFieldLocator
{
    public required bool Relative { get; set; }
    public required string[] Path { get; set; }

    public INode Locate(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var route = Path.SkipLast(1).ToArray();
        var label = Path.DefaultIfEmpty().Last();

        if (label is null)
            return cursor;

        object current = gff.Root;

        if (Relative)
        {
            if (cursor is FieldNode fieldNode)
                current = fieldNode.Struct.GetFields().Single(x => x.Label == fieldNode.Label).value;
            if (cursor is RootNode rootNode)
                current = rootNode.Struct;
            if (cursor is ListStructNode listStructNode)
                current = listStructNode.List.ElementAt(listStructNode.Index);
        }

        foreach (var node in route)
        {
            if (current is GFFStruct currentNode)
            {
                current = currentNode.GetFields().Single(x => x.Label == node);

                // Null
                // >1
                if (current is not GFFList && current is not GFFStruct)
                    throw new Exception(); // TODO
            }
            else if (current is GFFList list)
            {
                var index = int.Parse(node);
                current = list.ElementAt(index);
            }
            else
            {
                throw new InvalidOperationException(); 
            }
        }

        if (current is GFFStruct @struct)
        {
            //if (!@struct.GetFields().Any(x => x.Label == label))
            //    throw new Exception(); // TODO

            return new FieldNode
            {
                Struct = @struct,
                Label = label,
            };
        }
        else if (current is GFFList list)
        {
            return new ListStructNode
            {
                List = list,
                Index = int.Parse(label),
            };
        }
        else
        {
            throw new Exception(); // TODO
        }
    }
}

public interface INode
{
    public bool IsStruct { get; }
}
public class RootNode : INode
{
    public bool IsStruct => true;

    public required GFFStruct Struct { get; init; }
}
public class FieldNode : INode
{
    public bool IsStruct => Struct.GetFields().FirstOrDefault(x => x.Label == Label).value is GFFStruct;

    public required GFFStruct Struct { get; init; }
    public required string Label { get; init; }
}
public class ListStructNode : INode
{
    public bool IsStruct => true;

    public required GFFList List { get; init; }
    public required int Index { get; init; }
}


public interface IGFFModifier
{
    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory);
}
public class EditUInt8Modifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<byte> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetUInt8(fieldNode.Label, value);
    }
}
public class EditUInt16Modifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<ushort> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetUInt16(fieldNode.Label, value);
    }
}
public class EditUInt32Modifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<uint> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetUInt32(fieldNode.Label, value);
    }
}
public class EditUInt64Modifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<ulong> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetUInt64(fieldNode.Label, value);
    }
}
public class EditInt8Modifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<sbyte> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetInt8(fieldNode.Label, value);
    }
}
public class EditInt16Modifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<short> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetInt16(fieldNode.Label, value);
    }
}
public class EditInt32Modifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<int> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetInt32(fieldNode.Label, value);
    }
}
public class EditInt64Modifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<long> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetInt64(fieldNode.Label, value);
    }
}
public class EditSingleModifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<float> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetSingle(fieldNode.Label, value);
    }
}
public class EditDoubleModifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<double> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetDouble(fieldNode.Label, value);
    }
}
public class EditResRefModifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<ResRef> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetResRef(fieldNode.Label, value);
    }
}
public class EditStringModifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<string> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetString(fieldNode.Label, value);
    }
}
public class EditLocalizedStringModifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<LocalisedString> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetLocalisedString(fieldNode.Label, value);
    }
}
public class EditLocalizedStringStringRefModifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<int> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        var locstring = fieldNode.Struct.GetLocalisedString(fieldNode.Label) ?? new();
        locstring.StringRef = value;
        fieldNode.Struct.SetLocalisedString(fieldNode.Label, locstring);
    }
}
public class EditBinaryModifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<byte[]> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetBinary(fieldNode.Label, value);
    }
}
public class EditVector3Modifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<Vector3> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetVector3(fieldNode.Label, value);
    }
}
public class EditVector4Modifier : IGFFModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<Vector4> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetVector4(fieldNode.Label, value);
    }
}




















public class SetStructModifier : IGFFModifier
{
    public required IFieldLocator Parent { get; set; }
    public required IValue<int> StructID { get; set; }
    public required List<IGFFModifier> Modifiers { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var node = Parent.Locate(gff, cursor, installation, memory);
        var structID = StructID.Get(gff, installation, memory);

        if (node is FieldNode fieldNode)
        {
            fieldNode.Struct.SetStruct(fieldNode.Label, structID);
        }
        else if (node is ListStructNode listStructNode)
        {
            if (listStructNode.Index == -1)
            {
                listStructNode.List.Add(structID);
                node = new ListStructNode()
                {
                    List = listStructNode.List,
                    Index = listStructNode.List.Count() - 1
                };
            }
            else
                listStructNode.List.ElementAt(listStructNode.Index);

        }

        Modifiers.ForEach(x => x.Apply(gff, node, installation, memory));
    }
}
public class SetListModifier : IGFFModifier
{
    public required IFieldLocator Parent { get; set; }
    public required List<IGFFModifier> Modifiers { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var node = Parent.Locate(gff, cursor, installation, memory);

        if (node is not FieldNode fieldNode)
            throw new NotImplementedException();

        if (fieldNode.Struct.GetList(fieldNode.Label) is null)
            fieldNode.Struct.SetList(fieldNode.Label);

        Modifiers.ForEach(x => x.Apply(gff, node, installation, memory));
    }
}
