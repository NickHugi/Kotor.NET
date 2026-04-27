using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Patcher.For2DA;
using Kotor.NET.Resources.Kotor2DA;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF;

public class PatchGFF
{
    public required ILocateResource TakeFrom { get; set; }
    public required ILocateResource SaveTo { get; set; }
    public ICollection<IModifier> Modifiers { get; set; } = [];
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
        var twoda = new TwoDA(); // TODO

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
    public Field Locate(GFF gff, Installation installation, PatcherMemory memory);
}
public class ByPathFieldLocator : IFieldLocator
{
    public required string[] Path { get; set; }

    public Field Locate(GFF gff, Installation installation, PatcherMemory memory)
    {
        var route = Path.SkipLast(1).ToArray();
        var label = Path.Last();

        object current = gff.Root;
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
            if (!@struct.GetFields().Any(x => x.Label == label))
                throw new Exception(); // TODO

            return new Field
            {
                Struct = @struct,
                Label = label,
            };
        }
        else
        {
            throw new Exception(); // TODO
        }
    }
}
public class Field
{
    // TODO - redo gff api to make this unnecessary
    public required GFFStruct Struct { get; init; }
    public required string Label { get; init; }
}


public interface IModifier
{
    public void Apply(GFF gff, Installation installation, PatcherMemory memory);
}
public class EditUInt8Modifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<byte> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        field.Struct.SetUInt8(field.Label, value);
    }
}
public class EditUInt16Modifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<ushort> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        field.Struct.SetUInt16(field.Label, value);
    }
}
public class EditUInt32Modifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<uint> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        field.Struct.SetUInt32(field.Label, value);
    }
}
public class EditUInt64Modifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<ulong> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        field.Struct.SetUInt64(field.Label, value);
    }
}
public class EditInt8Modifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<sbyte> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        field.Struct.SetInt8(field.Label, value);
    }
}
public class EditInt16Modifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<short> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        field.Struct.SetInt16(field.Label, value);
    }
}
public class EditInt32Modifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<int> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        field.Struct.SetInt32(field.Label, value);
    }
}
public class EditInt64Modifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<long> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        field.Struct.SetInt64(field.Label, value);
    }
}
public class EditSingleModifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<float> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        field.Struct.SetSingle(field.Label, value);
    }
}
public class EditDoubleModifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<double> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        field.Struct.SetDouble(field.Label, value);
    }
}
public class EditResRefModifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<ResRef> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        field.Struct.SetResRef(field.Label, value);
    }
}
public class EditStringModifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<string> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        field.Struct.SetString(field.Label, value);
    }
}
public class EditLocalizedStringStringRefModifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<int> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        var locstring = field.Struct.GetLocalisedString(field.Label) ?? new();
        locstring.StringRef = value;
        field.Struct.SetLocalisedString(field.Label, locstring);
    }
}
public class EditBinaryModifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<byte[]> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        field.Struct.SetBinary(field.Label, value);
    }
}
public class EditVector3Modifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<Vector3> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        field.Struct.SetVector3(field.Label, value);
    }
}
public class EditVector4Modifier : IModifier
{
    public required IFieldLocator Field { get; set; }
    public required IValue<Vector4> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);

        field.Struct.SetVector4(field.Label, value);
    }
}
