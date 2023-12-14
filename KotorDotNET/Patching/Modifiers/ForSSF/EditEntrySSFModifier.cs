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
        public IValue StringRef { get; set; }

        public EditEntrySSFModifier(CreatureSound creatureSound, IValue value)
        {
            CreatureSound = creatureSound;
            StringRef = value;
        }

        public void Apply(SSF target, IMemory memory, ILogger logger)
        {
            try
            {
                var stringRef = StringRef.GetValue(memory, target, CreatureSound);
                target.Set(CreatureSound, stringRef);
            }
            catch (ApplyModifierException ex)
            {
                logger.Warning(ex.Message);
            }
        }
    }
}
