using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.FileFormats.KotorNCS;

namespace KotorDotNET.Compiler.Compilation.Expression.Constant
{
    public class ObjectConstantExpression : IExpression
    {
        public ushort Value { get; set; }

        public ObjectConstantExpression(ushort value)
        {
            Value = value;
        }

        public DataType GetDataType()
        {
            return DataType.String;
        }

        public void Compile(SymbolTable symbolTable, NCS ncs)
        {
            ncs.Add(new NCSInstruction.CONSTO(Value));
        }
    }
}
