using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        var modifiers = context.edit_item_mod().Select(Visit).OfType<IModifier>().ToList();

        return new PatchUTI()
        {
            TakeFrom = new HardcodedLocateResource(),
            SaveTo = new HardcodedLocateResource(),
            Modifiers = modifiers,
        };
    }

    public override object VisitUTI_SetField_BaseItem_Int32([NotNull] KotorPatchingLanguageParser.UTI_SetField_BaseItem_Int32Context context)
    {
        return new EditInt32Modifier
        {
            Field = new ByPathFieldLocator()
            {
                Path = ["BaseItem"]
            },
            Value = (IValue<int>)context.gff_value_int32()
        };
    }
    public override object VisitUTI_SetField_BaseItem_2DALabelLookup([NotNull] KotorPatchingLanguageParser.UTI_SetField_BaseItem_2DALabelLookupContext context)   
    {
        var label = GetStringLiteralText(context.STRING_LITERAL());

        return new EditInt32Modifier
        {
            Field = new ByPathFieldLocator()
            {
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

    public override object VisitUTI_SetField_LocalizedName_LocalizedString([NotNull] KotorPatchingLanguageParser.UTI_SetField_LocalizedName_LocalizedStringContext context)
    {
        return new EditLocalizedStringModifier
        {
            Field = new ByPathFieldLocator()
            {
                Path = ["LocalizedName"]
            },
            Value = ((IValue<LocalisedString>)context.gff_value_locstring())
        };
    }
}
