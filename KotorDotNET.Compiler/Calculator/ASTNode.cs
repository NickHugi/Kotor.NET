using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Compiler.Calculator
{
    public interface ASTNode
    {
        void Parse(SymbolTable symbolTable);
    }
}
