using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Common.Data;

public class LocalisedString
{
    public int StringRef { get; set; } = -1;

    private List<LocalisedSubstring> _substrings { get; set; } = new();

    public LocalisedString()
    {
    }
    public LocalisedString(int stringref)
    {
        StringRef = stringref;
    }
    public LocalisedString(IEnumerable<LocalisedSubstring> substrings)
    {
        _substrings = substrings.ToList();
    }

    public IEnumerable<LocalisedSubstring> AllSubstrings()
    {
        return _substrings.ToList();
    }
    public void RemoveAllSubstrings()
    {
        _substrings.Clear();
    }
    public void SetSubstring(Language language, Gender gender, string text)
    {
        RemoveSubstring(language, gender);
        _substrings.Add(new(language, gender, text));
    }
    public string? GetSubstring(Language language, Gender gender)
    {
        return _substrings.FirstOrDefault(x => x.Language == language && x.Gender == gender)?.Text;
    }
    public void RemoveSubstring(Language language, Gender gender)
    {
        _substrings.Where(x => x.Language != language && x.Gender != gender).ToList();
    }
    public bool HasSubstring(Language language, Gender gender)
    {
        return _substrings.Any(x => x.Language == language && x.Gender == gender);
    }
    public int Count()
    {
        return _substrings.Count();
    }
}
