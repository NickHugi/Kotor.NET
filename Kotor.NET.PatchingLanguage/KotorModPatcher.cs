using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Kotor.NET.Encapsulations;
using Kotor.NET.Patcher.ForUTI;
using Kotor.NET.PatchingLanguage.Visitor;
using Kotor.NET.Resources.KotorUTI;

namespace Kotor.NET.PatchingLanguage;

public static class KotorModPatcher
{
    public static void Install(string patchDirectory, Installation installation, string script)
    {
        AntlrInputStream inputStream = new AntlrInputStream(script);
        KotorPatchingLanguageLexer speakLexer = new KotorPatchingLanguageLexer(inputStream);
        CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
        KotorPatchingLanguageParser parser = new KotorPatchingLanguageParser(commonTokenStream);
        var context = parser.script();
        KotorPatchingLanguageVisitor visitor = new KotorPatchingLanguageVisitor();
        var patch = ((List<object>)visitor.Visit(context)).OfType<PatchUTI>().ToList();
        patch.First().Apply(installation, new(), patchDirectory);
    }
}
