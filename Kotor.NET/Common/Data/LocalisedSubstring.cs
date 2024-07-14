namespace Kotor.NET.Common.Data;

public class LocalisedSubstring
{
    public Language Language { get; set; }
    public Gender Gender { get; set; }
    public string Text { get; set; } = "";

    public int StringID => ((int)Gender) + ((int)Language * 2);

    public LocalisedSubstring(Language language, Gender gender, string text)
    {
        Language = language;
        Gender = gender;
        Text = text;
    }
}
