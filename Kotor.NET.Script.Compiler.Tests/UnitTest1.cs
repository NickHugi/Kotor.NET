using Kotor.NET.Script.Compiler.Calculator;
using Kotor_NET_Script_Compiler.Calculator;


namespace Kotor.NET.Script.Compiler.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Calc.Run("void helloworld();s");
    }
}
