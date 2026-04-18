using Antlr4.Runtime.Misc;
using Kotor.NET.Patcher.For2DA;

namespace Kotor.NET.PatchingLanguage;

public class KotorPatchingLanguageVisitor : KotorPatchingLanguageBaseVisitor<object>
{
    public override object VisitScript(KotorPatchingLanguageParser.ScriptContext context)
    {     
        
        // NameContext name = context.name();
        // OpinionContext opinion = context.opinion();
        // SpeakLine line = new SpeakLine() { Person = name.GetText(), Text = opinion.GetText().Trim('"') };
        // Lines.Add(line);
        // return line;
        return null;
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

    public override object VisitTwoDAOverrideRow([NotNull] KotorPatchingLanguageParser.TwoDAOverrideRowContext context)
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
        }
    }
    #endregion

    #region Appearance
    public override object VisitEditAppearance([NotNull] KotorPatchingLanguageParser.EditAppearanceContext context)
    {
        return new EditAppearance
        {
            TakeFrom = new HardcodedFile(),
            SaveTo = new HardcodedFile(),
            Modifiers = context.edit_appearance_mod()
        };
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
}
