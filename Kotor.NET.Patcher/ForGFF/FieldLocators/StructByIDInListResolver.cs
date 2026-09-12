using Kotor.NET.Encapsulations;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF.FieldLocators;

public class StructByIDInListResolver : INodeResolver
{
    public int StructID { get; set; }

    public INode Locate(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        if (cursor is FieldNode fieldNode)
        {
            var listField = fieldNode.Struct.GetList(fieldNode.Label);
            var @struct = listField.FirstOrDefault(x => x.ID == StructID) ?? listField.Add(StructID);

            return new ListStructNode()
            {
                Index = listField.IndexOf(@struct),
                List = listField,
            };
        }
        throw new NotImplementedException();
    }
}
