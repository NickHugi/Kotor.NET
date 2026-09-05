using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Kotor.NET.Patcher;
using Kotor.NET.Patcher.LocateResource;

namespace Kotor.NET.PatchingLanguage.Visitor;

public partial class KotorPatchingLanguageVisitor : KotorPatchingLanguageBaseVisitor<object>
{
    public override object VisitFile_Target_Override([NotNull] KotorPatchingLanguageParser.File_Target_OverrideContext context)
    {
        return new OverrideLocateContainer();
    }

    public override object VisitFile_Target_Module([NotNull] KotorPatchingLanguageParser.File_Target_ModuleContext context)
    {
        return new ModuleLocateContainer()
        {
            ModuleID = GetStringLiteralText(context.STRING_LITERAL())
        };
    }
}
