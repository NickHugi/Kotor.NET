using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Kotor.NET.Patcher;

namespace Kotor.NET.PatchingLanguage.Visitor;

public partial class KotorPatchingLanguageVisitor : KotorPatchingLanguageBaseVisitor<object>
{
    public override object VisitFile_Operation_CreateOrReplace([NotNull] KotorPatchingLanguageParser.File_Operation_CreateOrReplaceContext context)
    {
        return new CreateOrReplaceFileOperation();
    }

    public override object VisitFile_Operation_CreateOrModify([NotNull] KotorPatchingLanguageParser.File_Operation_CreateOrModifyContext context)
    {
        return new CreateOrModifyFileOperation();
    }
}
