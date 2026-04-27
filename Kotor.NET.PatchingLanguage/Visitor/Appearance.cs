using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Kotor.NET.Patcher;
using Kotor.NET.Patcher.For2DA;

namespace Kotor.NET.PatchingLanguage.Visitor;

public partial class KotorPatchingLanguageVisitor : KotorPatchingLanguageBaseVisitor<object>
{
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
            TakeFrom = new HardcodedLocateResource(), // TODO
            SaveTo = new HardcodedLocateResource(), // TODO
            Modifiers = [new RowModifier
            {
                TargetRow = targetRow,
                Assignments = assignments,
            }]
        };
    }
}
