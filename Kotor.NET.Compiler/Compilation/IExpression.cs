using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Compiler.Calculator;

namespace Kotor.NET.Compiler.Compilation
{
    public interface IExpression : ASTNode
    {
        public DataType GetDataType();
    }
}
