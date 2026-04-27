using System.Data.Common;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.For2DA;

public class Patch2DA : IPatch
{
    public required ILocateResource TakeFrom { get; set; }
    public required ILocateResource SaveTo { get; set; }
    public ICollection<IModifier> Modifiers { get; set; } = [];

    public void Apply(PatcherMemory memory)
    {
        var twoda = TwoDA.FromBytes(TakeFrom.Load());

        foreach (var modifier in Modifiers)
        {
            modifier.Apply(twoda, memory);
        }

        SaveTo.Save(TwoDA.ToBytes(twoda));
    }
}
public class EditAppearance : Patch2DA
{
    public EditAppearance()
    {
        TakeFrom = new HardcodedLocateResource();
        SaveTo = new HardcodedLocateResource();
    }
}


public interface IRowLocator
{
    public TwoDARow? Locate(TwoDA twoda, PatcherMemory memory);
}
public class ByCellValueRowLocator : IRowLocator
{
    public required string Column { get; set; }
    public required string Value { get; set; }

    public TwoDARow? Locate(TwoDA twoda, PatcherMemory memory)
    {
        return twoda.GetRows().Where(x => x.GetCell(Column).AsString() == Value).SingleOrDefault();
    }
}
public class NoRowLocator : IRowLocator
{
    public TwoDARow? Locate(TwoDA twoda, PatcherMemory memory)
    {
        return null;
    }
}

public interface IValue
{
    public string Get(TwoDA twoda, PatcherMemory memory);
}
public class ConstantValue : IValue
{
    public required string Text { get; set; }

    public string Get(TwoDA twoda, PatcherMemory memory)
    {
        return Text;
    }
}

public interface IModifier
{
    public void Apply(TwoDA twoda, PatcherMemory memory);
}
public class RowModifier : IModifier
{
    public required IRowLocator TargetRow { get; set; }
    public ICollection<IAssignment> Assignments { get; set; } = [];

    public void Apply(TwoDA twoda, PatcherMemory memory)
    {
        var row = TargetRow.Locate(twoda, memory);

        if (row is null)
        {
            var rowHeader = twoda.GetRows().Length.ToString();
            row = twoda.AddRow(rowHeader);
        }

        foreach (var assignment in Assignments)
        {
            assignment.Apply(twoda, row, memory);
        }
    }
}

public interface IAssignment
{
    public void Apply(TwoDA twoda, TwoDARow target, PatcherMemory memory);
}
public class EditCellAssignment : IAssignment
{
    public required IValue CellValue { get; set; }
    public required string Column { get; set; }

    public void Apply(TwoDA twoda, TwoDARow target, PatcherMemory memory)
    {
        var value = CellValue.Get(twoda, memory);
        target.GetCell(Column).SetString(value);
    }
}
public class CopyRowAssignment : IAssignment
{
    public required IRowLocator SourceRow { get; set; }

    public void Apply(TwoDA twoda, TwoDARow target, PatcherMemory memory)
    {
        var source = SourceRow.Locate(twoda, memory);

        foreach (var column in twoda.GetColumns())
        {
            var value = source.GetCell(column).AsString();
            target.GetCell(column).SetString(value);
        }
    }
}
