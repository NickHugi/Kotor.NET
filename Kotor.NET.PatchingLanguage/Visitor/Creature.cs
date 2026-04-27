using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Kotor.NET.Patcher;
using Kotor.NET.Patcher.ForGFF;

namespace Kotor.NET.PatchingLanguage.Visitor;

public partial class KotorPatchingLanguageVisitor : KotorPatchingLanguageBaseVisitor<object>
{
    public override object VisitEditCreature([NotNull] KotorPatchingLanguageParser.EditCreatureContext context)
    {
        var modifiers = context.edit_creature_mod().Select(Visit).OfType<IModifier>().ToList();

        return new EditCreature
        {
            TakeFrom = new HardcodedLocateResource(), // TODO
            SaveTo = new HardcodedLocateResource(), // TODO
            Modifiers = modifiers,
        };
    }

    public override object VisitEditCreatureAppearance([NotNull] KotorPatchingLanguageParser.EditCreatureAppearanceContext context)
    {
        return new EditUInt16Modifier
        {
            Field = new ByPathFieldLocator
            {
                Path = ["Appearance_Type"]
            },
            Value = (IValue<ushort>)context.gff_value_uint16(),
        };
    }
    public override object VisitEditCreatureAppearanceFromLabel([NotNull] KotorPatchingLanguageParser.EditCreatureAppearanceFromLabelContext context)
    {
        return new EditUInt16Modifier
        {
            Field = new ByPathFieldLocator
            {
                Path = ["Appearance_Type"]
            },
            Value = new TwoDARowIndexValue<ushort>
            {
                ResRef = "appearance",
                SearchColumn = "label",
                SearchForCell = GetStringLiteralText(context.STRING_LITERAL()),
            },
        };
    }

    public override object VisitEditCreaturePortrait([NotNull] KotorPatchingLanguageParser.EditCreaturePortraitContext context)
    {
        return new EditUInt16Modifier
        {
            Field = new ByPathFieldLocator
            {
                Path = ["PortraitId"]
            },
            Value = (IValue<ushort>)context.gff_value_uint16(),
        };
    }
    public override object VisitEditCreaturePortraitFromLabel([NotNull] KotorPatchingLanguageParser.EditCreaturePortraitFromLabelContext context)
    {
        return new EditUInt16Modifier
        {
            Field = new ByPathFieldLocator
            {
                Path = ["PortraitId"]
            },
            Value = new TwoDARowIndexValue<ushort>
            {
                ResRef = "appearance",
                SearchColumn = "label",
                SearchForCell = GetStringLiteralText(context.STRING_LITERAL()),
            },
        };
    }

    public override object VisitEditCreatureGender([NotNull] KotorPatchingLanguageParser.EditCreatureGenderContext context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldLocator
            {
                Path = ["Gender"]
            },
            Value = (IValue<byte>)context.gff_value_uint8(),
        };
    }
    public override object VisitEditCreatureGenderFromKeyword([NotNull] KotorPatchingLanguageParser.EditCreatureGenderFromKeywordContext context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldLocator
            {
                Path = ["Gender"]
            },
            Value = new ConstantValue<byte>()
            {
                Value = GetStringLiteralText(context.GetText()) switch
                {
                    "male" => 0,
                    "female" => 1,
                    "both" => 2,
                    "other" => 3,
                    "none" => 4,
                    _ => throw new InvalidOperationException()
                }
            },
        };
    }

    public override object race






}
