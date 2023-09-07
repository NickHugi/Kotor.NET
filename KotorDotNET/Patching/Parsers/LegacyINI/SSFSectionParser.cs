// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser.Model;
using KotorDotNET.Common.Creature;
using KotorDotNET.Exceptions;
using KotorDotNET.FileFormats.Kotor2DA;
using KotorDotNET.FileFormats.KotorSSF;
using KotorDotNET.Patching.Modifiers.ForSSF;
using KotorDotNET.Patching.Modifiers.ForSSF.Values;

namespace KotorDotNET.Patching.Parsers.LegacyINI
{
    public class SSFSectionParser
    {
        private IniData _ini;
        private KeyDataCollection _section;

        public SSFSectionParser(IniData ini, KeyDataCollection section)
        {
            _ini = ini;
            _section = section;
        }

        public List<IModifier<SSF>> Parse()
        {
            var modifiers = new List<IModifier<SSF>>();

            foreach (var section in _section)
            {
                var sound = GetSoundFromString(section.KeyName);
                var stringref = GetValueFromString(section.Value);
                modifiers.Add(new EditEntrySSFModifier(sound, stringref));
            }

            return modifiers;
        }

        private CreatureSound GetSoundFromString(string text)
        {
            if (text == "BattleCry1") return CreatureSound.BATTLE_CRY_1;
            else if(text == "BattleCry2") return CreatureSound.BATTLE_CRY_2;
            else if(text == "BattleCry3") return CreatureSound.BATTLE_CRY_3;
            else if(text == "BattleCry4") return CreatureSound.BATTLE_CRY_4;
            else if(text == "BattleCry5") return CreatureSound.BATTLE_CRY_5;
            else if(text == "BattleCry6") return CreatureSound.BATTLE_CRY_6;
            else if(text == "Select1") return CreatureSound.SELECT_1;
            else if(text == "Select2") return CreatureSound.SELECT_2;
            else if(text == "Select3") return CreatureSound.SELECT_3;
            else if(text == "AttackGrunt1") return CreatureSound.ATTACK_GRUNT_1;
            else if(text == "AttackGrunt2") return CreatureSound.ATTACK_GRUNT_2;
            else if(text == "AttackGrunt3") return CreatureSound.ATTACK_GRUNT_3;
            else if(text == "PainGrunt1") return CreatureSound.PAIN_GRUNT_1;
            else if(text == "PainGrunt2") return CreatureSound.PAIN_GRUNT_2;
            else if(text == "") return CreatureSound.LOW_HEALTH;
            else if(text == "") return CreatureSound.DEAD;
            else if(text == "") return CreatureSound.CRITICAL_HIT;
            else if(text == "") return CreatureSound.TARGET_IMMUNE;
            else if(text == "") return CreatureSound.LAY_MINE;
            else if(text == "") return CreatureSound.DISARM_MINE;
            else if(text == "") return CreatureSound.BEGIN_STEALTH;
            else if(text == "") return CreatureSound.BEGIN_SEARCH;
            else if(text == "") return CreatureSound.BEGIN_UNLOCK;
            else if(text == "") return CreatureSound.UNLOCK_FAILED;
            else if(text == "") return CreatureSound.UNLOCK_SUCCEEDED;
            else if(text == "") return CreatureSound.SEPARATED_FROM_PARTY;
            else if(text == "") return CreatureSound.REJOINED_PARTY;
            else if(text == "") return CreatureSound.POISONED;
            else throw new PatchingParserException("Unrecognized SSF creature sound.");
        }

        private IValue GetValueFromString(string text)
        {
            if (text.StartsWith("2DAMEMORY"))
            {
                int number;
                var isValidNumber = int.TryParse(text.Substring(9), out number);
                if (isValidNumber)
                {
                    return new TwoDAMemoryValue(number);
                }
                else
                {
                    throw new PatchingParserException(""); // TODO
                }
            }
            else if (text.StartsWith("StrRef"))
            {
                int number;
                var isValidNumber = int.TryParse(text.Substring(6), out number);
                if (isValidNumber)
                {
                    return new TwoDAMemoryValue(number);
                }
                else
                {
                    throw new PatchingParserException(""); // TODO
                }
            }
            else
            {
                throw new PatchingParserException(""); // TODO
            }
        }
    }
}
