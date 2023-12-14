using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.FileFormats.KotorNCS;

namespace KotorDotNET.Compiler.Compilation.Expression.Constant
{
    public class StringConstantExpression : IExpression
    {
        public string Value { get; set; }

        public StringConstantExpression(string value)
        {
            Value = value;
        }

        public DataType GetDataType()
        {
            return DataType.String;
        }

        public void Compile(SymbolTable symbolTable, NCS ncs)
        {
            ncs.Add(new NCSInstruction.CONSTS(Value));
        }
    }
}
