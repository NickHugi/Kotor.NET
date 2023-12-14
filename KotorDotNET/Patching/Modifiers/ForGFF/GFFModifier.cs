using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.FileFormats.KotorGFF;

namespace KotorDotNET.Patching.Modifiers.ForGFF
{
    public interface IGFFModifier : IModifier<GFF>
    {
        void SetNavigator(GFFNavigator navigator);
    }
}
