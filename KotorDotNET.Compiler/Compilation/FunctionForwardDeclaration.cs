using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Compiler.Calculator;

namespace KotorDotNET.Compiler.Compilation
{
    public class FunctionForwardDeclaration : ASTNode
    {
        public DataType ReturnType { get; }
        public string Symbol { get; }
        public List<FunctionParameter> Parameters { get; }

        public FunctionForwardDeclaration(DataType returnType, string symbol, List<FunctionParameter> parameters)
        {
            ReturnType = returnType;
            Symbol = symbol;
            Parameters = parameters;
        }

        public void Parse(SymbolTable symbolTable) => throw new NotImplementedException();
    }
}
