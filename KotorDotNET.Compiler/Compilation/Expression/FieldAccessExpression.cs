using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.FileFormats.KotorNCS;

namespace KotorDotNET.Compiler.Compilation.Expression.Binary
{
    public class FieldAccessExpression : IExpression
    {

        public FieldAccessExpression()
        {

        }

        public DataType GetDataType()
        {
            return null;
        }

        public void Compile(SymbolTable symbolTable, NCS ncs)
        {
            
        }
    }
}
