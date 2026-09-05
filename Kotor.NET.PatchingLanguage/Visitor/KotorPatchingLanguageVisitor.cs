using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using Kotor.NET.Common;

namespace Kotor.NET.PatchingLanguage.Visitor;

public partial class KotorPatchingLanguageVisitor : KotorPatchingLanguageBaseVisitor<object>
{
    public override object VisitScript(KotorPatchingLanguageParser.ScriptContext context)
    {
        return context.instruction().Select(Visit).ToList();
    }


    private string GetStringLiteralText(ITerminalNode node)
    {
        return GetStringLiteralText(node.GetText());
    }
    private string GetStringLiteralText(string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        if (str.Length >= 2 && str[0] == '"' && str[^1] == '"')
            return str.Substring(1, str.Length - 2);
        return str;
    }

    private Language GetTLKLanguage(ITerminalNode node)
    {
        return node.GetText() switch
        {
            "english" => Language.English,
            _ => throw new NotSupportedException("Unsupported language")
        };
    }

    private Gender GetTLKGender(ITerminalNode node)
    {
        return node.GetText() switch
        {
            "male" => Gender.Male,
            "female" => Gender.Female,
            _ => throw new NotSupportedException("Invalid gender")
        };
    }

    private string GetMemoryTokenText(ITerminalNode node)
    {
        return node.GetText().Substring(1);
    }

    private Vector3 GetVector3LiteralValue(ITerminalNode node)
    {
        var text = node.GetText();
        var components = text.Split(',', '\n', '\r', ' ', '\t').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        var x = float.Parse(components[0]);
        var y = float.Parse(components[1]);
        var z = float.Parse(components[2]);
        return new Vector3(x, y, z);
    }

    private Vector4 GetVector4LiteralValue(ITerminalNode node)
    {
        var text = node.GetText();
        var components = text.Split(',', '\n', '\r', ' ', '\t').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        var x = float.Parse(components[0]);
        var y = float.Parse(components[1]);
        var z = float.Parse(components[2]);
        var w = float.Parse(components[3]);
        return new Vector4(x, y, z, w);
    }

    
}
