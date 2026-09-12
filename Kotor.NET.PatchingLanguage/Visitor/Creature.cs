using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Kotor.NET.Common.Data;
using Kotor.NET.Patcher;
using Kotor.NET.Patcher.FileOperation;
using Kotor.NET.Patcher.ForGFF;
using Kotor.NET.Patcher.ForGFF.FieldLocators;
using Kotor.NET.Patcher.ForGFF.Modifiers;
using Kotor.NET.Patcher.ForGFF.Values;
using Kotor.NET.Patcher.ForUTI;
using Kotor.NET.Patcher.LocateResource;
using static KotorPatchingLanguageParser;

namespace Kotor.NET.PatchingLanguage.Visitor;

public partial class KotorPatchingLanguageVisitor : KotorPatchingLanguageBaseVisitor<object>
{
    public override object VisitEditCreature([NotNull] KotorPatchingLanguageParser.EditCreatureContext context)
    {
        var resref = GetStringLiteralText(context.STRING_LITERAL());
        var fileOperation = (IFileOperation)Visit(context.file_operation());
        var takeFrom = (ILocateContainer)Visit(context.file_source());
        var saveTo = (ILocateContainer)Visit(context.file_target());
        var modifiers = context.edit_creature_mod().Select(Visit).OfType<IGFFModifier>().ToList();

        return new PatchUTI()
        {
            ResRef = resref,
            ResourceType = ResourceType.UTC,
            FileOperation = fileOperation,
            TakeFrom = takeFrom,
            SaveTo = saveTo,
            Modifiers = modifiers,
        };
    }

    public override object VisitUTC_AppearanceType_SetField_Int32([NotNull] KotorPatchingLanguageParser.UTC_AppearanceType_SetField_Int32Context context)
    {
        return new EditUInt16Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Appearance_Type"]
            },
            Value = (IValue<ushort>)Visit(context.gff_value_uint16()),
        };
    }
    public override object VisitUTC_AppearanceType_SetField_2DALabelLookup([NotNull] KotorPatchingLanguageParser.UTC_AppearanceType_SetField_2DALabelLookupContext context)
    {
        var label = GetStringLiteralText(context.STRING_LITERAL());

        return new EditUInt16Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Appearance_Type"]
            },
            Value = new TwoDARowIndexValue<ushort>()
            {
                ResRef = "appearance",
                SearchColumn = "label",
                SearchForCell = label
            }
        };
    }

    public override object VisitUTC_BlindSpot_SetField_Single([NotNull] KotorPatchingLanguageParser.UTC_BlindSpot_SetField_SingleContext context)
    {
        return new EditSingleModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["BlindSpot"]
            },
            Value = (IValue<float>)Visit(context.gff_value_single()),
        };
    }

    public override object VisitUTC_Cha_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_Cha_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Cha"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_ChallengeRating_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_ChallengeRating_SetField_UInt8Context context)
    {
        return new EditSingleModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ChallengeRating"]
            },
            Value = (IValue<float>)Visit(context.gff_value_single()),
        };
    }

    public override object VisitUTC_Con_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_Con_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Con"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_Conversation_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_Conversation_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Conversation"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_CurrentForce_SetField_Int16([NotNull] KotorPatchingLanguageParser.UTC_CurrentForce_SetField_Int16Context context)
    {
        return new EditInt16Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["CurrentForce"]
            },
            Value = (IValue<short>)Visit(context.gff_value_int16()),
        };
    }

    public override object VisitUTC_CurrentHitPoints_SetField_Int16([NotNull] KotorPatchingLanguageParser.UTC_CurrentHitPoints_SetField_Int16Context context)
    {
        return new EditInt16Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["CurrentHitPoints"]
            },
            Value = (IValue<short>)Visit(context.gff_value_int16()),
        };
    }

    public override object VisitUTC_Description_SetField_LocalizedString([NotNull] KotorPatchingLanguageParser.UTC_Description_SetField_LocalizedStringContext context)
    {
        return new EditLocalizedStringModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Description"]
            },
            Value = (IValue<LocalisedString>)Visit(context.gff_value_locstring()),
        };
    }

    public override object VisitUTC_Dex_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_Dex_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Dex"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_Disarmable_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_Disarmable_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Disarmable"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_FactionID_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_FactionID_SetField_UInt8Context context)
    {
        return new EditUInt16Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["FactionID"]
            },
            Value = (IValue<ushort>)Visit(context.gff_value_uint16()),
        };
    }
    public override object VisitUTC_FactionID_SetField_2DALabelLookup([NotNull] KotorPatchingLanguageParser.UTC_FactionID_SetField_2DALabelLookupContext context)
    {
        var label = GetStringLiteralText(context.STRING_LITERAL());

        return new EditUInt16Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["FactionID"]
            },
            Value = new TwoDARowIndexValue<ushort>()
            {
                ResRef = "repute",
                SearchColumn = "label",
                SearchForCell = label
            }
        };
    }

    public override object VisitUTC_FirstName_SetField_LocalizedString([NotNull] KotorPatchingLanguageParser.UTC_FirstName_SetField_LocalizedStringContext context)
    {
        return new EditLocalizedStringModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["FirstName"]
            },
            Value = (IValue<LocalisedString>)Visit(context.gff_value_locstring()),
        };
    }

    public override object VisitUTC_ForcePoints_SetField_Int16([NotNull] KotorPatchingLanguageParser.UTC_ForcePoints_SetField_Int16Context context)
    {
        return new EditInt16Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ForcePoints"]
            },
            Value = (IValue<short>)Visit(context.gff_value_int16()),
        };
    }

    public override object VisitUTC_Gender_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_Gender_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Gender"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_GoodEvil_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_GoodEvil_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["GoodEvil"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_HitPoints_SetField_Int16([NotNull] KotorPatchingLanguageParser.UTC_HitPoints_SetField_Int16Context context)
    {
        return new EditInt16Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["HitPoints"]
            },
            Value = (IValue<short>)Visit(context.gff_value_int16()),
        };
    }

    public override object VisitUTC_Hologram_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_Hologram_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Hologram"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_IgnoreCrePath_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_IgnoreCrePath_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["IgnoreCrePath"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_Int_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_Int_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Int"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_IsPC_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_IsPC_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["IsPC"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_LastName_SetField_LocalizedString([NotNull] KotorPatchingLanguageParser.UTC_LastName_SetField_LocalizedStringContext context)
    {
        return new EditLocalizedStringModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["LastName"]
            },
            Value = (IValue<LocalisedString>)Visit(context.gff_value_locstring()),
        };
    }

    public override object VisitUTC_MaxHitPoints_SetField_Int16([NotNull] KotorPatchingLanguageParser.UTC_MaxHitPoints_SetField_Int16Context context)
    {
        return new EditInt16Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["MaxHitPoints"]
            },
            Value = (IValue<short>)Visit(context.gff_value_int16()),
        };
    }

    public override object VisitUTC_Min1HP_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_Min1HP_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Min1HP"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_MultiplierSet_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_MultiplierSet_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["MultiplierSet"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_NaturalAC_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_NaturalAC_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["NaturalAC"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_NoPermanentDeath_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_NoPermanentDeath_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["NoPermDeath"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_NotReorienting_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_NotReorienting_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["NotReorienting"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_PartInteract_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_PartInteract_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["PartyInteract"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_PerceiptionRange_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_PerceiptionRange_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["PerceptionRange"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_Plot_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_Plot_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Plot"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_Phenotype_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_Phenotype_SetField_UInt8Context context)
    {
        return new EditInt32Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Phenotype"]
            },
            Value = (IValue<int>)Visit(context.gff_value_int32()),
        };
    }
    public override object VisitUTC_Phenotype_SetField_2DALabelLookup([NotNull] KotorPatchingLanguageParser.UTC_Phenotype_SetField_2DALabelLookupContext context)
    {
        var label = GetStringLiteralText(context.STRING_LITERAL());

        return new EditInt32Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Phenotype"]
            },
            Value = new TwoDARowIndexValue<int>()
            {
                ResRef = "phenotype",
                SearchColumn = "label",
                SearchForCell = label
            }
        };
    }

    public override object VisitUTC_PortraitID_SetField_Int32([NotNull] KotorPatchingLanguageParser.UTC_PortraitID_SetField_Int32Context context)
    {
        return new EditInt32Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["PortraitId"]
            },
            Value = (IValue<int>)Visit(context.gff_value_int32()),
        };
    }

    public override object VisitUTC_PortraitID_SetField_2DALabelLookup([NotNull] KotorPatchingLanguageParser.UTC_PortraitID_SetField_2DALabelLookupContext context)
    {
        var label = GetStringLiteralText(context.STRING_LITERAL());

        return new EditInt32Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["PortraitId"]
            },
            Value = new TwoDARowIndexValue<int>()
            {
                ResRef = "portraits",
                SearchColumn = "label",
                SearchForCell = label
            }
        };
    }

    public override object VisitUTC_Race_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_Race_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Race"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }
    public override object VisitUTC_Race_SetField_2DALabelLookup([NotNull] KotorPatchingLanguageParser.UTC_Race_SetField_2DALabelLookupContext context)
    {
        var label = GetStringLiteralText(context.STRING_LITERAL());

        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Race"]
            },
            Value = new TwoDARowIndexValue<byte>()
            {
                ResRef = "racialtypes",
                SearchColumn = "label",
                SearchForCell = label
            }
        };
    }

    public override object VisitUTC_ScriptAttacked_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_ScriptAttacked_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ScriptAttacked"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_ScriptDamaged_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_ScriptDamaged_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ScriptDamaged"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_ScriptDeath_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_ScriptDeath_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ScriptDeath"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_ScriptDialogue_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_ScriptDialogue_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ScriptDialogue"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_ScriptDisturbed_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_ScriptDisturbed_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ScriptDisturbed"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_ScriptEndDialog_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_ScriptEndDialog_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ScriptEndDialogu"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_ScriptEndRound_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_ScriptEndRound_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ScriptEndRound"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_ScriptHeartbeat_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_ScriptHeartbeat_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ScriptHeartbeat"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_ScriptBlocked_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_ScriptBlocked_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ScriptOnBlocked"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_ScriptNotice_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_ScriptNotice_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ScriptOnNotice"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_ScriptRested_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_ScriptRested_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ScriptRested"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_ScriptSpawn_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_ScriptSpawn_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ScriptSpawn"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_ScriptSpellAt_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_ScriptSpellAt_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ScriptSpellAt"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_ScriptUserDefine_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_ScriptUserDefine_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["ScriptUserDefine"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_SoundsetFile_SetField_UInt16([NotNull] KotorPatchingLanguageParser.UTC_SoundsetFile_SetField_UInt16Context context)
    {
        return new EditUInt16Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["SoundSetFile"]
            },
            Value = (IValue<ushort>)Visit(context.gff_value_uint16()),
        };
    }
    public override object VisitUTC_SoundsetFile_SetField_2DALabelLookup([NotNull] KotorPatchingLanguageParser.UTC_SoundsetFile_SetField_2DALabelLookupContext context)
    {
        var label = GetStringLiteralText(context.STRING_LITERAL());

        return new EditUInt16Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["SoundSetFile"]
            },
            Value = new TwoDARowIndexValue<ushort>()
            {
                ResRef = "soundset",
                SearchColumn = "label",
                SearchForCell = label
            }
        };
    }

    public override object VisitUTC_Str_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_Str_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Str"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_Subrace_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_Subrace_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["SubraceIndex"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_Tag_SetField_String([NotNull] KotorPatchingLanguageParser.UTC_Tag_SetField_StringContext context)
    {
        return new EditStringModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Tag"]
            },
            Value = (IValue<string>)Visit(context.gff_value_string()),
        };
    }

    public override object VisitUTC_WalkRate_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_WalkRate_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["WalkRate"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_WalkRate_SetField_2DALabelLookup([NotNull] KotorPatchingLanguageParser.UTC_WalkRate_SetField_2DALabelLookupContext context)
    {
        var label = GetStringLiteralText(context.STRING_LITERAL());

        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["WalkRate"]
            },
            Value = new TwoDARowIndexValue<byte>()
            {
                ResRef = "creaturespeed",
                SearchColumn = "label",
                SearchForCell = label
            }
        };
    }

    public override object VisitUTC_Wis_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_Wis_SetField_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["Wis"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    public override object VisitUTC_FortBonus_SetField_Int16([NotNull] KotorPatchingLanguageParser.UTC_FortBonus_SetField_Int16Context context)
    {
        return new EditInt16Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["fortbonus"]
            },
            Value = (IValue<short>)Visit(context.gff_value_int16()),
        };
    }

    public override object VisitUTC_RefBonus_SetField_Int16([NotNull] KotorPatchingLanguageParser.UTC_RefBonus_SetField_Int16Context context)
    {
        return new EditInt16Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["refbonus"]
            },
            Value = (IValue<short>)Visit(context.gff_value_int16()),
        };
    }

    public override object VisitUTC_WillBonus_SetField_Int16([NotNull] KotorPatchingLanguageParser.UTC_WillBonus_SetField_Int16Context context)
    {
        return new EditInt16Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = false,
                Path = ["willbonus"]
            },
            Value = (IValue<short>)Visit(context.gff_value_int16()),
        };
    }

    // Inventory
    public override object VisitUTC_AddItem([NotNull] KotorPatchingLanguageParser.UTC_AddItemContext context)
    {
        var setFields = context.utc_add_inventory_mod().Select(Visit).OfType<IGFFModifier>().ToList();
        var setStruct = new SetStructModifier()
        {
            Parent = new ByPathFieldResolver()
            {
                Relative = true,
                Path = ["-1"]
            },
            StructID = new NextStructIDInList(),
            Modifiers = setFields
        };
        var setList = new SetListModifier
        {
            Parent = new ByPathFieldResolver()
            {
                Relative = false,
                Path = ["ItemList"]
            },
            Modifiers = [setStruct],
        };

        return setList;
    }

    public override object VisitUTC_AddItem_SetField_ResRef([NotNull] KotorPatchingLanguageParser.UTC_AddItem_SetField_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = true,
                Path = ["InventoryRes"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_AddItem_SetField_Dropable([NotNull] KotorPatchingLanguageParser.UTC_AddItem_SetField_DropableContext context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = true,
                Path = ["Dropable"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    // Equipment
    public override object VisitUTC_SetEquipment([NotNull] KotorPatchingLanguageParser.UTC_SetEquipmentContext context)
    {
        var setFields = context.utc_set_equipment_mod().Select(Visit).OfType<IGFFModifier>().ToList();
        var slot = (int)Visit(context.equipment_slot());
        var setStruct = new SetStructModifier()
        {
            Parent = new StructByIDInListResolver(),
            StructID = new ConstantValue<int>() { Value = (int)Math.Pow(2, slot) },
            Modifiers = setFields
        };
        var setList = new SetListModifier
        {
            Parent = new ByPathFieldResolver()
            {
                Relative = false,
                Path = ["Equip_ItemList"]
            },
            Modifiers = [setStruct],
        };

        return setList;
    }

    public override object VisitUTC_SetEquipment_ResRef_ResRef([NotNull] KotorPatchingLanguageParser.UTC_SetEquipment_ResRef_ResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = true,
                Path = ["EquippedRes"]
            },
            Value = (IValue<ResRef>)Visit(context.gff_value_resref()),
        };
    }

    public override object VisitUTC_SetEquipment_Dropable_UInt8([NotNull] KotorPatchingLanguageParser.UTC_SetEquipment_Dropable_UInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = true,
                Path = ["Dropable"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8()),
        };
    }

    // Feats
    public override object VisitUTC_AddFeat_UInt16([NotNull] KotorPatchingLanguageParser.UTC_AddFeat_UInt16Context context)
    {
        var setField = new EditUInt16Modifier()
        {
            Field = new ByPathFieldResolver()
            {
                Relative = true,
                Path = ["Feat"]
            },
            Value = (IValue<ushort>)Visit(context.gff_value_uint16())
        };
        var setStruct = new SetStructModifier()
        {
            Parent = new StructByIDInListResolver(),
            StructID = new ConstantValue<int>() { Value = 1 },
            Modifiers = [setField]
        };
        var setList = new SetListModifier
        {
            Parent = new ByPathFieldResolver()
            {
                Relative = false,
                Path = ["FeatList"]
            },
            Modifiers = [setStruct],
        };

        return setList;
    }
    public override object VisitUTC_AddFeat_2DALabelLookup([NotNull] KotorPatchingLanguageParser.UTC_AddFeat_2DALabelLookupContext context)
    {
        var label = GetStringLiteralText(context.STRING_LITERAL());

        var setField = new EditUInt16Modifier()
        {
            Field = new ByPathFieldResolver()
            {
                Relative = true,
                Path = ["Feat"]
            },
            Value = new TwoDARowIndexValue<ushort>()
            {
                ResRef = "feat",
                SearchColumn = "label",
                SearchForCell = label
            }
        };
        var setStruct = new SetStructModifier()
        {
            Parent = new StructByIDInListResolver(),
            StructID = new ConstantValue<int>() { Value = 1 },
            Modifiers = [setField]
        };
        var setList = new SetListModifier
        {
            Parent = new ByPathFieldResolver()
            {
                Relative = false,
                Path = ["FeatList"]
            },
            Modifiers = [setStruct],
        };

        return setList;
    }

    // Classes
    public override object VisitUTC_Class_SetNew([NotNull] KotorPatchingLanguageParser.UTC_Class_SetNewContext context)
    {
        var setFields = context.utc_class_mod().Select(Visit).OfType<IGFFModifier>().ToList();
        var setStruct = new SetStructModifier()
        {
            Parent = new StructByNewInListResolver(),
            StructID = new ConstantValue<int>() { Value = 2 },
            Modifiers = setFields
        };
        var setList = new SetListModifier
        {
            Parent = new ByPathFieldResolver()
            {
                Relative = false,
                Path = ["ClassList"]
            },
            Modifiers = [setStruct],
        };

        return setList;
    }
    public override object VisitUTC_Class_SetFirst([NotNull] KotorPatchingLanguageParser.UTC_Class_SetFirstContext context)
    {
        var setFields = context.utc_class_mod().Select(Visit).OfType<IGFFModifier>().ToList();
        var setStruct = new SetStructModifier()
        {
            Parent = new StructByIndexInListResolver() { Index = 0 },
            StructID = new ConstantValue<int>() { Value = 2 },
            Modifiers = setFields
        };
        var setList = new SetListModifier
        {
            Parent = new ByPathFieldResolver()
            {
                Relative = false,
                Path = ["ClassList"]
            },
            Modifiers = [setStruct],
        };

        return setList;
    }
    public override object VisitUTC_Class_SetSecond([NotNull] KotorPatchingLanguageParser.UTC_Class_SetSecondContext context)
    {
        var setFields = context.utc_class_mod().Select(Visit).OfType<IGFFModifier>().ToList();
        var setStruct = new SetStructModifier()
        {
            Parent = new StructByIndexInListResolver() { Index = 1 },
            StructID = new ConstantValue<int>() { Value = 2 },
            Modifiers = setFields
        };
        var setList = new SetListModifier
        {
            Parent = new ByPathFieldResolver()
            {
                Relative = false,
                Path = ["ClassList"]
            },
            Modifiers = [setStruct],
        };

        return setList;
    }

    public override object VisitUTC_Class_Type_SetField_Int32([NotNull] KotorPatchingLanguageParser.UTC_Class_Type_SetField_Int32Context context)
    {
        return new EditInt32Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = true,
                Path = ["Class"]
            },
            Value = (IValue<int>)Visit(context.gff_value_int32()),
        };
    }
    public override object VisitUTC_Class_Type_SetField_2DALookup([NotNull] KotorPatchingLanguageParser.UTC_Class_Type_SetField_2DALookupContext context)
    {
        var label = GetStringLiteralText(context.STRING_LITERAL());

        return new EditInt32Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = true,
                Path = ["Class"]
            },
            Value = new TwoDARowIndexValue<int>()
            {
                ResRef = "classes",
                SearchColumn = "label",
                SearchForCell = label
            }
        };
    }

    public override object VisitUTC_Class_Level_SetField_Int16([NotNull] KotorPatchingLanguageParser.UTC_Class_Level_SetField_Int16Context context)
    {
        return new EditInt16Modifier
        {
            Field = new ByPathFieldResolver
            {
                Relative = true,
                Path = ["ClassLevel"]
            },
            Value = (IValue<short>)Visit(context.gff_value_int16()),
        };
    }

    // Powers
    public override object VisitUTC_Class_AddPower_UInt16([NotNull] KotorPatchingLanguageParser.UTC_Class_AddPower_UInt16Context context)
    {
        var setFields = new List<IGFFModifier>()
        {
            new EditUInt16Modifier
            {
                Field = new ByPathFieldResolver
                {
                    Relative = true,
                    Path = ["Spell"]
                },
                Value = (IValue<ushort>)Visit(context.gff_value_uint16())
            },
            new EditUInt8Modifier
            {
                Field = new ByPathFieldResolver
                {
                    Relative = true,
                    Path = ["SpellFlags"]
                },
                Value = new ConstantValue<byte>() { Value = 1 }
            },
            new EditUInt8Modifier
            {
                Field = new ByPathFieldResolver
                {
                    Relative = true,
                    Path = ["SpellMetaMagic"]
                },
                Value = new ConstantValue<byte>() { Value = 0 }
            },
        };
        var setStruct = new SetStructModifier()
        {
            Parent = new StructByNewInListResolver(),
            StructID = new ConstantValue<int>() { Value = 3 },
            Modifiers = setFields
        };
        var setList = new SetListModifier
        {
            Parent = new ByPathFieldResolver()
            {
                Relative = true,
                Path = ["KnownList0"]
            },
            Modifiers = [setStruct],
        };

        return setList;
    }
    public override object VisitUTC_Class_AddPower_UInt16_2DALabelLookup([NotNull] KotorPatchingLanguageParser.UTC_Class_AddPower_UInt16_2DALabelLookupContext context)
    {
        var label = GetStringLiteralText(context.STRING_LITERAL());

        var setFields = new List<IGFFModifier>()
        {
            new EditUInt16Modifier
            {
                Field = new ByPathFieldResolver
                {
                    Relative = true,
                    Path = ["Spell"]
                },
                Value = new TwoDARowIndexValue<ushort>()
                {
                    ResRef = "spells",
                    SearchColumn = "label",
                    SearchForCell = label
                }
            },
            new EditUInt8Modifier
            {
                Field = new ByPathFieldResolver
                {
                    Relative = true,
                    Path = ["SpellFlags"]
                },
                Value = new ConstantValue<byte>() { Value = 1 }
            },
            new EditUInt8Modifier
            {
                Field = new ByPathFieldResolver
                {
                    Relative = true,
                    Path = ["SpellMetaMagic"]
                },
                Value = new ConstantValue<byte>() { Value = 0 }
            },
        };
        var setStruct = new SetStructModifier()
        {
            Parent = new StructByNewInListResolver(),
            StructID = new ConstantValue<int>() { Value = 3 },
            Modifiers = setFields
        };
        var setList = new SetListModifier
        {
            Parent = new ByPathFieldResolver()
            {
                Relative = true,
                Path = ["KnownList0"]
            },
            Modifiers = [setStruct],
        };

        return setList;
    }

    // Skills
    private object SetSkill(int index, Gff_value_uint8Context value)
    {
        var setField = new EditUInt8Modifier()
        {
            Field = new ByPathFieldResolver
            {
                Relative = true,
                Path = ["Rank"]
            },
            Value = (IValue<byte>)Visit(value),
        };
        var setStruct = new SetStructModifier()
        {
            Parent = new StructByIndexInListResolver() { Index = index, FillPrevious = true },
            StructID = new ConstantValue<int>() { Value = 0 },
            Modifiers = [setField]
        };
        var setList = new SetListModifier
        {
            Parent = new ByPathFieldResolver()
            {
                Relative = false,
                Path = ["SkillList"]
            },
            Modifiers = [setStruct],
        };
        return setList;
    }
    public override object VisitUTC_Skills_ComputerUse_SetField_UInt8([NotNull] KotorPatchingLanguageParser.UTC_Skills_ComputerUse_SetField_UInt8Context context)
    {
        return SetSkill(0, context.gff_value_uint8());
    }
    public override object VisitUTC_Skills_Demolutions_SetField_UInt8([NotNull] UTC_Skills_Demolutions_SetField_UInt8Context context)
    {
        return SetSkill(1, context.gff_value_uint8());
    }
    public override object VisitUTC_Skills_Stealth_SetField_UInt8([NotNull] UTC_Skills_Stealth_SetField_UInt8Context context)
    {
        return SetSkill(2, context.gff_value_uint8());
    }
    public override object VisitUTC_Skills_Awareness_SetField_UInt8([NotNull] UTC_Skills_Awareness_SetField_UInt8Context context)
    {
        return SetSkill(3, context.gff_value_uint8());
    }
    public override object VisitUTC_Skills_Persuade_SetField_UInt8([NotNull] UTC_Skills_Persuade_SetField_UInt8Context context)
    {
        return SetSkill(4, context.gff_value_uint8());
    }
    public override object VisitUTC_Skills_Repair_SetField_UInt8([NotNull] UTC_Skills_Repair_SetField_UInt8Context context)
    {
        return SetSkill(5, context.gff_value_uint8());
    }
    public override object VisitUTC_Skills_Security_SetField_UInt8([NotNull] UTC_Skills_Security_SetField_UInt8Context context)
    {
        return SetSkill(6, context.gff_value_uint8());
    }
    public override object VisitUTC_Skills_TreatInjury_SetField_UInt8([NotNull] UTC_Skills_TreatInjury_SetField_UInt8Context context)
    {
        return SetSkill(7, context.gff_value_uint8());
    }
}
