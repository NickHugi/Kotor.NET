using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Kotor.NET.Common.Data;
using Kotor.NET.Patcher.CopyFiles;
using Kotor.NET.Patcher.LocateResource;

namespace Kotor.NET.PatchingLanguage.Visitor;

public partial class KotorPatchingLanguageVisitor : KotorPatchingLanguageBaseVisitor<object>
{
    public override object VisitCopyFiles([NotNull] KotorPatchingLanguageParser.CopyFilesContext context)
    {
        return new PatchCopyFiles()
        {
            SourceContainer = new PatchDirectoryLocateContainer(),
            TargetContainer = (ILocateContainer)Visit(context.copy_files_target()),
            Commands = context.copy_files_command().Select(Visit).OfType<CopyFileCommand>().ToList()
        };
    }

    public override object VisitCopy_Files_Command_ChangeName([NotNull] KotorPatchingLanguageParser.Copy_Files_Command_ChangeNameContext context)
    {
        var sourceFilename = GetStringLiteralText(context.STRING_LITERAL(0));
        var targetFilename = GetStringLiteralText(context.STRING_LITERAL(1));
        var resourceType = ResourceType.FromFilepath(sourceFilename);

        return new CopyFileCommand()
        {
            ResourceType = resourceType,
            SourceFileName = sourceFilename,
            TargetFileName = targetFilename
        };
    }
    public override object VisitCopy_Files_Command_KeepName([NotNull] KotorPatchingLanguageParser.Copy_Files_Command_KeepNameContext context)
    {
        var sourceFilename = GetStringLiteralText(context.STRING_LITERAL());
        var resourceType = ResourceType.FromFilepath(sourceFilename);

        return new CopyFileCommand()
        {
            ResourceType = resourceType,
            SourceFileName = sourceFilename,
            TargetFileName = sourceFilename
        };
    }

    public override object VisitCopy_Files_Target_Module([NotNull] KotorPatchingLanguageParser.Copy_Files_Target_ModuleContext context)
    {
        return new ModuleLocateContainer()
        {
            ModuleID = GetStringLiteralText(context.STRING_LITERAL())
        };
    }
    public override object VisitCopy_Files_Target_Override([NotNull] KotorPatchingLanguageParser.Copy_Files_Target_OverrideContext context)
    {
        return new OverrideLocateContainer();
    }
}
