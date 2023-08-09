using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Common;

public class ValidationIssue(string message)
{
    public string Message { get; set; } = message;
}
