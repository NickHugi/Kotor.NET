using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Common.Conversation;
using KotorDotNET.Common.Data;

namespace KotorDotNET.FileFormats.KotorTLK
{
    public class TLK
    {
        public Language Language { get; set; }
        public List<TLKEntry> Entries { get; set; }

        public TLK()
        {
            Language = Language.ENGLISH;
            Entries = new();
        }
        public TLK(Language language)
        {
            Language = language;
            Entries = new();
        }

        public void Add(string text)
        {
            Entries.Add(new TLKEntry(text));
        }
        public void Add(string text, ResRef resref)
        {
            Entries.Add(new TLKEntry(text, resref));
        }

        public TLKEntry Get(int index)
        {
            return Entries[index];
        }
    }

    public class TLKEntry
    {
        public string Text { get; set; }
        public ResRef ResRef { get; set; }

        public TLKEntry(string text, ResRef resRef)
        {
            Text = text;
            ResRef = resRef;
        }

        public TLKEntry(string text)
        {
            Text = text;
            ResRef = new();
        }
    }
}
