using Kotor.NET.Encapsulations;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF.FieldLocators;

public class StructByIndexInListResolver : INodeResolver
{
    public bool FillPrevious { get; set; }
    public required int Index { get; set; }

    public INode Locate(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        if (cursor is FieldNode fieldNode)
        {
            var listField = fieldNode.Struct.GetList(fieldNode.Label);

            if (FillPrevious)
            {
                var numToAdd = Math.Max(0, Index - listField.Count + 1);
                for (int i = 0; i  < numToAdd; i++)
                {
                    listField.Add();
                }
            }

            var @struct = listField.ElementAt(Index);

            return new ListStructNode()
            {
                Index = listField.IndexOf(@struct),
                List = listField,
            };
        }
        throw new NotImplementedException();
    }
}
