using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.FileFormats.KotorNCS;

namespace KotorDotNET.Compiler.Calculator
{
    public interface ASTNode
    {
        void Compile(SymbolTable symbolTable, NCS ncs);
    }
}
