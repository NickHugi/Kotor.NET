using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.FileFormats.KotorNCS;

namespace KotorDotNET.Compiler.Compilation.Expression
{
    public class BitwiseNegationExpression : IExpression
    {
        public IExpression Expression { get; set; }

        public BitwiseNegationExpression(IExpression expression)
        {
            Expression = expression;
        }

        public DataType GetDataType()
        {
            return DataType.Float;
        }

        public void Compile(SymbolTable symbolTable, NCS ncs)
        {
            Expression.Compile(symbolTable, ncs);

            var type = Expression.GetDataType();

            if (type == DataType.Int)
            {
                ncs.Add(new NCSInstruction.COMPI());
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
