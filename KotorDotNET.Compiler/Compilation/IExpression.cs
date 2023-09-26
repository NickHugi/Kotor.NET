using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Compiler.Calculator;

namespace KotorDotNET.Compiler.Compilation
{
    public interface IExpression : ASTNode
    {
        public DataType GetDataType();
    }
}
