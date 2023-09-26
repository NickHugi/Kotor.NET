using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Compiler.Calculator;

namespace KotorDotNET.Compiler.Compilation
{
    public class GlobalVariableInitialization : ASTNode
    {
        public DataType DataType { get; }
        public string Identifier { get; }
        public IExpression Expression { get; }

        public GlobalVariableInitialization(DataType dataType, string identifier, IExpression expression)
        {
            DataType = dataType;
            Identifier = identifier;
            Expression = expression;
        }

        public void Parse(SymbolTable symbolTable)
        {

        }

        public override string ToString()
        {
            return $"{DataType.Type} {Identifier} = {Expression};";
        }
    }
}
