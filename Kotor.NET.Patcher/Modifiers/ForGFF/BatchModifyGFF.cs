using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Patcher.Modifiers.ForGFF;

public class BatchModifyGFF
{
    public string Destination { get; set; }
    public string SourceFolder { get; set; }
    public string SourceFile { get; set; }
    public bool ReplaceFile { get; set; }
    public string SaveAs { get; set; }

}

public enum OverrideType
{
    Ignore,
    Warn,
    Rename
}
