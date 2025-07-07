using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Common.Localization;

public readonly struct TalkTableString
{
    public StringRef StringRef { get; }
    public string Text { get; }
    public ResRef Sound { get; }

    public TalkTableString(StringRef stringref, string text, ResRef resref)
    {
        StringRef = stringref;
        Text = text;
        Sound = resref;
    }
}
