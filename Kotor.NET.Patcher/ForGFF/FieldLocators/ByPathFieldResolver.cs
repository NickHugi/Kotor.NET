using Kotor.NET.Encapsulations;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF.FieldLocators;

public class ByPathFieldResolver : INodeResolver
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
