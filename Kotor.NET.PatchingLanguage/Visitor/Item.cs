using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Build.Tasks;
using Antlr4.Runtime.Misc;
using Kotor.NET.Common.Data;
using Kotor.NET.Patcher;
using Kotor.NET.Patcher.ForGFF;
using Kotor.NET.Patcher.ForUTI;

namespace Kotor.NET.PatchingLanguage.Visitor;

public partial class KotorPatchingLanguageVisitor : KotorPatchingLanguageBaseVisitor<object>
{
    public override object VisitEditItem([NotNull] KotorPatchingLanguageParser.EditItemContext context)
    {
        var resref = GetStringLiteralText(context.STRING_LITERAL());
        var fileOperation = (IFileOperation)Visit(context.file_operation());
        var takeFrom = (ILocateContainer)Visit(context.file_source());
        var saveTo = (ILocateContainer)Visit(context.file_target());
        var modifiers = context.edit_item_mod().Select(Visit).OfType<IGFFModifier>().ToList();

        return new PatchUTI()
        {
            ResRef = resref,
            ResourceType = ResourceType.UTI,
            FileOperation = fileOperation,
            TakeFrom = takeFrom,
            SaveTo = saveTo,
            Modifiers = modifiers,
        };
    }

    public override object VisitUTI_BaseItem_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_BaseItem_SetField_GFFValueContext context)
    {
        return new EditInt32Modifier
        {
            Field = new ByPathFieldLocator()
            {
                Relative = false,
                Path = ["BaseItem"]
            },
            Value = (IValue<int>)Visit(context.gff_value_int32())
        };
    }
    public override object VisitUTI_BaseItem_SetField_2DALabelLookup([NotNull] KotorPatchingLanguageParser.UTI_BaseItem_SetField_2DALabelLookupContext context)
    {
        var label = GetStringLiteralText(context.STRING_LITERAL());

        return new EditInt32Modifier
        {
            Field = new ByPathFieldLocator()
            {
                Relative = false,
                Path = ["BaseItem"]
            },
            Value = new TwoDARowIndexValue<int>()
            {
                ResRef = "baseitems",
                SearchColumn = "label",
                SearchForCell = label
            }
        };
    }

    public override object VisitUTI_LocalizedName_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_LocalizedName_SetField_GFFValueContext context)
    {
        return new EditLocalizedStringModifier
        {
            Field = new ByPathFieldLocator()
            {
                Relative = false,
                Path = ["LocalizedName"]
            },
            Value = (ConstantValue<LocalisedString>)Visit(context.gff_value_locstring())
        };
    }

    public override object VisitUTI_Description_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_Description_SetField_GFFValueContext context)
    {
        return new EditLocalizedStringModifier
        {
            Field = new ByPathFieldLocator()
            {
                Relative = false,
                Path = ["Description"]
            },
            Value = new ConstantValue<LocalisedString>
            {
                Value = (LocalisedString)Visit(context.gff_value_locstring())
            }
        };
    }

    public override object VisitUTI_Tag_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_Tag_SetField_GFFValueContext context)
    {
        return new EditStringModifier
        {
            Field = new ByPathFieldLocator()
            {
                Relative = false,
                Path = ["Tag"]
            },
            Value = (IValue<string>)Visit(context.gff_value_string())
        };
    }

    public override object VisitUTI_Charges_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_Charges_SetField_GFFValueContext context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = false,
                Path = ["Charges"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8())
        };
    }

    public override object VisitUTI_MaxCharges_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_MaxCharges_SetField_GFFValueContext context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = false,
                Path = ["MaxCharges"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8())
        };
    }

    public override object VisitUTI_Cost_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_Cost_SetField_GFFValueContext context)
    {
        return new EditUInt32Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = false,
                Path = ["Cost"]
            },
            Value = (IValue<uint>)Visit(context.gff_value_uint32())
        };
    }

    public override object VisitUTI_StackSize_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_StackSize_SetField_GFFValueContext context)
    {
        return new EditUInt16Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = false,
                Path = ["StackSize"]
            },
            Value = (IValue<ushort>)Visit(context.gff_value_uint16()    )
        };
    }

    public override object VisitUTI_Plot_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_Plot_SetField_GFFValueContext context)
    {
        return new EditInt8Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = false,
                Path = ["Plot"]
            },
            Value = (IValue<sbyte>)Visit(context.gff_value_int8())
        };
    }

    public override object VisitUTI_Plot_SetField_Bool([NotNull] KotorPatchingLanguageParser.UTI_Plot_SetField_BoolContext context)
    {
        return new EditInt8Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = false,
                Path = ["Plot"]
            },
            Value = new ConstantValue<sbyte>
            {
                Value = context.BOOL_LITERAL().GetText() == "true" ? (sbyte)1 : (sbyte)0
            }
        };
    }

    public override object VisitUTI_ModelVariation_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_ModelVariation_SetField_GFFValueContext context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = false,
                Path = ["ModelVariation"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8())
        };
    }

    public override object VisitUTI_TextureVariation_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_TextureVariation_SetField_GFFValueContext context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = false,
                Path = ["TextureVar"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8())
        };
    }

    // Property
    public override object VisitUTI_AddProperties([NotNull] KotorPatchingLanguageParser.UTI_AddPropertiesContext context)
    {
        var setFields = context.uti_property_mod().Select(Visit).OfType<IGFFModifier>().ToList();
        var setStruct = new SetStructModifier()
        {
            Parent = new ByPathFieldLocator()
            {
                Relative = true,
                Path = ["-1"]
            },
            StructID = new ConstantValue<int>() { Value = 0 },
            Modifiers = setFields
        };
        var setList = new SetListModifier
        {
            Parent = new ByPathFieldLocator()
            {
                Relative = false,
                Path = ["PropertiesList"]
            },
            Modifiers = [setStruct],
        };

        return setList;
    }

    public override object VisitUTI_Property_PropertyName_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_Property_PropertyName_SetField_GFFValueContext context)
    {
        return new EditUInt16Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = true,
                Path = ["PropertyName"]
            },
            Value = (IValue<ushort>)Visit(context.gff_value_uint16())
        };
    }

    public override object VisitUTI_Property_SubType_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_Property_SubType_SetField_GFFValueContext context)
    {
        return new EditUInt16Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = true,
                Path = ["Subtype"]
            },
            Value = (IValue<ushort>)Visit(context.gff_value_uint16()    )
        };
    }

    public override object VisitUTI_Property_ChanceAppear_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_Property_ChanceAppear_SetField_GFFValueContext context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = true,
                Path = ["ChanceAppear"]
            },
            Value = (IValue<byte>)  Visit(context.gff_value_uint8())
        };
    }

    public override object VisitUTI_Property_CostTable_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_Property_CostTable_SetField_GFFValueContext context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = true,
                Path = ["CostTable"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8())
        };
    }

    public override object VisitUTI_Property_CostValue_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_Property_CostValue_SetField_GFFValueContext context)
    {
        return new EditUInt16Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = true,
                Path = ["CostValue"]
            },
            Value = (IValue<ushort>)Visit(context.gff_value_uint16())
        };
    }

    public override object VisitUTI_Property_Param1_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_Property_Param1_SetField_GFFValueContext context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = true,
                Path = ["Param1"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8())
        };
    }

    public override object VisitUTI_Property_Param1Value_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_Property_Param1Value_SetField_GFFValueContext context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = true,
                Path = ["Param1Value"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8())
        };
    }

    public override object VisitUTI_Property_UpgradeType_SetField_GFFValue([NotNull] KotorPatchingLanguageParser.UTI_Property_UpgradeType_SetField_GFFValueContext context)
    {
        return new EditUInt8Modifier
        {
            Field = new ByPathFieldLocator
            {
                Relative = true,
                Path = ["UpgradeType"]
            },
            Value = (IValue<byte>)Visit(context.gff_value_uint8())
        };
    }
}
