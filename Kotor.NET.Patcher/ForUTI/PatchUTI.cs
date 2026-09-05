using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Encapsulations;
using Kotor.NET.Patcher.ForGFF;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForUTI;

public class PatchUTI : PatchGFF
{

}

//public class EditProperties : IGFFModifier
//{
//    public required IFieldLocator Field { get; set; }
//    public required List<IGFFModifier> Modifiers { get; set; }

//    public void Apply(GFF gff, GFFStruct targetStruct, Installation installation, PatcherMemory memory)
//    {
//        var propertiesList = gff.Root.GetList("PropertiesList");

//        if (propertiesList is null)
//        {
//            propertiesList = gff.Root.GetList("PropertiesList");
//        }

        

//        var field = Field.Locate(gff, installation, memory);
//        var value = Value.Get(gff, installation, memory);

//        field.Struct.SetInt32(field.Label, value);
//    }
//}
