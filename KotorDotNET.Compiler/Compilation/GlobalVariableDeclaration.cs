using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Compiler.Calculator;
using KotorDotNET.FileFormats.KotorNCS;

namespace KotorDotNET.Compiler.Compilation
{
    public class GlobalVariableDeclaration : ASTNode
    {
        public DataType DataType { get; }
        public string Identifier { get; }

        public GlobalVariableDeclaration(DataType dataType, string identifier)
        {
            DataType = dataType;
            Identifier = identifier;
        }

        public void Compile(SymbolTable symbolTable, NCS ncs)
        {
            symbolTable.DeclareGlobalVariable(Identifier, DataType);
        }

        public override string ToString()
        {
            return $"{DataType.Type} {Identifier};";
        }
    }
}
