using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Encapsulations;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF;

public class PatchGFF
{
    public required ILocateResource TakeFrom { get; set; }
    public required ILocateResource SaveTo { get; set; }
    public ICollection<IModifier> Modifiers { get; set; } = [];
}


public interface IValue<T>
{
    public T Get(GFF gff, Installation installation, PatcherMemory memory);
}
public class ConstantStringValue : IValue<string>
{
    public required string Value { get; set; }

    public string Get(GFF gff, Installation installation, PatcherMemory memory)
    {
        return Value;
    }
}

public interface IFieldAssignment
{
    public void Locate(GFF gff, Installation installation, PatcherMemory memory);
}
public class ByPathFieldAssignment : IFieldAssignment
{
    public required string[] Path { get; set; }

    public void Locate(GFF gff, Installation installation, PatcherMemory memory)
    {
        var route = Path.SkipLast(1).ToArray();
        var label = Path.Last();

        object current = gff.Root;
        foreach (var node in route)
        {
            if (current is GFFStruct @struct)
            {
                current = @struct.GetFields().Single(x => x.Label == node);

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
            //@struct.Set
        }
        else
        {
            throw new Exception(); // TODO
        }
    }
}


public interface IModifier
{
    public void Apply(GFF gff, Installation installation, PatcherMemory memory);
}

public class EditUInt8Modifier : IModifier
{
    public required IFieldAssignment Field { get; set; }
    public required IValue<byte> Value { get; set; }

    public void Apply(GFF gff, Installation installation, PatcherMemory memory)
    {
        //Field.Locate(gff, installation, memory);
        var value = Value.Get(gff, installation, memory);


    }
}
