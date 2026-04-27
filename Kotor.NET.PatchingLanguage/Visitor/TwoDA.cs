using System.Linq;
using Antlr4.Runtime.Misc;
using Kotor.NET.Patcher;
using Kotor.NET.Patcher.For2DA;
using Kotor.NET.Patcher.ForGFF;

namespace Kotor.NET.PatchingLanguage.Visitor;

public partial class KotorPatchingLanguageVisitor : KotorPatchingLanguageBaseVisitor<object>
{
    public override object VisitTwoDAAssignCell([NotNull] KotorPatchingLanguageParser.TwoDAAssignCellContext context)
    {
        return new EditCellAssignment
        {
            Column = GetStringLiteralText(context.STRING_LITERAL(0).GetText()),
            CellValue = new ConstantValue
            {
                Text = GetStringLiteralText(context.STRING_LITERAL(1).GetText())
            }
        };
    }

    public override object VisitTwoDATargetRow([NotNull] KotorPatchingLanguageParser.TwoDATargetRowContext context)
    {
        return new ByCellValueRowLocator
        {
            Column = GetStringLiteralText(context.STRING_LITERAL(0).GetText()),
            Value = GetStringLiteralText(context.STRING_LITERAL(1).GetText()),
        };
    }

    public override object VisitTwoDACopyRow([NotNull] KotorPatchingLanguageParser.TwoDACopyRowContext context)
    {
        return new CopyRowAssignment
        {
            SourceRow = new ByCellValueRowLocator
            {
                Column = GetStringLiteralText(context.STRING_LITERAL(0).GetText()),
                Value = GetStringLiteralText(context.STRING_LITERAL(1).GetText()),
            }
        };
    }
}
