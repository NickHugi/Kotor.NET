using System.Linq;
using Antlr4.Runtime.Misc;
using Kotor.NET.Patcher;
using Kotor.NET.Patcher.For2DA;

namespace Kotor.NET.PatchingLanguage;

public class KotorPatchingLanguageVisitor : KotorPatchingLanguageBaseVisitor<object>
{
    public override object VisitScript(KotorPatchingLanguageParser.ScriptContext context)
    {
        return context.instruction().Select(Visit).ToList();
    }

    #region 2DA
    public override object VisitTwoDAAssignCell([NotNull] KotorPatchingLanguageParser.TwoDAAssignCellContext context)
    {
        return new EditCellAssignment
        {
            Column = Unquote(context.STRING_LITERAL(0).GetText()),
            CellValue = new ConstantValue
            {
                Text = Unquote(context.STRING_LITERAL(1).GetText())
            }
        };
    }

    public override object VisitTwoDATargetRow([NotNull] KotorPatchingLanguageParser.TwoDATargetRowContext context)
    {
        return new ByCellValueRowLocator
        {
            Column = Unquote(context.STRING_LITERAL(0).GetText()),
            Value = Unquote(context.STRING_LITERAL(1).GetText()),
        };
    }

    public override object VisitTwoDACopyRow([NotNull] KotorPatchingLanguageParser.TwoDACopyRowContext context)
    {
        return new CopyRowAssignment
        {
            SourceRow = new ByCellValueRowLocator
            {
                Column = Unquote(context.STRING_LITERAL(0).GetText()),
                Value = Unquote(context.STRING_LITERAL(1).GetText()),
            }
        };
    }
    #endregion

    #region Appearance
    public override object VisitEditAppearance([NotNull] KotorPatchingLanguageParser.EditAppearanceContext context)
    {
        var targetRow = context.edit_appearance_mod().Select(Visit).OfType<IRowLocator>().SingleOrDefault() ?? new NoRowLocator();
        var assignments = context.edit_appearance_mod().Select(Visit).OfType<IAssignment>().ToList();

        if (targetRow is ByCellValueRowLocator rowLocator)
        {
            assignments.Add(new EditCellAssignment
            {
                Column = rowLocator.Column,
                CellValue = new ConstantValue { Text = rowLocator.Value },
            });
        }

        return new EditAppearance
        {
            TakeFrom = new HardcodedLocateResource(),
            SaveTo = new HardcodedLocateResource(),
            Modifiers = [new RowModifier
            {
                TargetRow = targetRow,
                Assignments = assignments,
            }]
        };
    }
    #endregion

    #region Creature
    public override object VisitEditCreature([NotNull] KotorPatchingLanguageParser.EditCreatureContext context)
    {
        return new EditCreature
        {

        }
    }
    #endregion


    private string Unquote(string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        if (str.Length >= 2 && str[0] == '"' && str[^1] == '"')
            return str.Substring(1, str.Length - 2);
        return str;
    }
}
