using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor_NET_Script_Compiler.Calculator;

namespace Kotor.NET.Script.Compiler.Calculator;

public static class Calc
{
    public static string Run(string s)
    {
        var parser = new CalculatorParser();
        parser.Parse(s);
        return "";
    }
}
