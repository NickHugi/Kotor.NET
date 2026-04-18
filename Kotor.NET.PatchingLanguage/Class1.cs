namespace Kotor.NET.PatchingLanguage;

public class KotorPatchingLanguageVisitor : KotorPatchingLanguageBaseVisitor<object>
{
    public override object VisitRoot(KotorPatchingLanguageParser.RootContext context)
    {            
        // NameContext name = context.name();
        // OpinionContext opinion = context.opinion();
        // SpeakLine line = new SpeakLine() { Person = name.GetText(), Text = opinion.GetText().Trim('"') };
        // Lines.Add(line);
        // return line;
        return null;
    }
}
