namespace Kotor.NET.Formats.AsciiTXI;

public class TXIAsciiField
{
    public string Instruction { get; set; } = "";
    public List<string> Values { get; } = new();
    public List<string[]> SubValues { get; } = new();

    private static readonly string[] _expandable = [TXIAsciiInstructions.UpperLeftCoords, TXIAsciiInstructions.LowerRightCoords, TXIAsciiInstructions.ChannelScale, TXIAsciiInstructions.ChannelTranslate];

    public TXIAsciiField(string instruction, IEnumerable<string> values)
    {
        Instruction = instruction;
        Values = values.ToList();
    }
    public TXIAsciiField(string instruction, IEnumerable<string> values, IEnumerable<string[]> subvalues)
    {
        Instruction = instruction;
        Values = values.ToList();
        SubValues = subvalues.ToList();
    }
    public TXIAsciiField(StreamReader reader, string[] tokens)
    {
        tokens = tokens.Select(x => x.ToLower()).ToArray();
        Instruction = tokens.First();
        Values = tokens.Skip(1).ToList();

        int.TryParse(tokens.ElementAt(1), out var count);
        if (_expandable.Contains(Instruction))
        {
            for (int i = 0; i < count; i++)
            {
                var line = reader.ReadLine();
                var subtokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                SubValues.Add(subtokens);
            }
        }
    }

    public void Write(StreamWriter writer)
    {
        writer.WriteLine(string.Join(' ', [Instruction, ..Values]));
        SubValues.ForEach(x => writer.WriteLine(string.Join(' ', x)));
    }
}
