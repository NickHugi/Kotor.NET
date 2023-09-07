using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Common.Creature;
using KotorDotNET.FileFormats.KotorSSF;

namespace KotorDotNET.Patching.Modifiers.ForSSF
{
    public class EditEntrySSFModifier : IModifier<SSF>
    {
        public CreatureSound CreatureSound { get; set; }
        public int StringRef { get; set; }

        public EditEntrySSFModifier(CreatureSound creatureSound, int stringRef)
        {
            CreatureSound = creatureSound;
            StringRef = stringRef;
        }

        public void Apply(SSF target, Memory memory, ILogger logger)
        {
            target.Set(CreatureSound, StringRef);
        }
    }
}
