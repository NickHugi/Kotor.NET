﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.FileFormats.KotorNCS;

namespace KotorDotNET.Compiler.Compilation.Expression.Binary
{
    public class LessThanOrEqualExpression : IExpression
    {
        public IExpression LHS { get; set; }
        public IExpression RHS { get; set; }

        public LessThanOrEqualExpression(IExpression lhs, IExpression rhs)
        {
            LHS = lhs;
            RHS = rhs;
        }

        public DataType GetDataType()
        {
            return DataType.Float;
        }

        public void Compile(SymbolTable symbolTable, NCS ncs)
        {
            LHS.Compile(symbolTable, ncs);
            RHS.Compile(symbolTable, ncs);

            var lhs = LHS.GetDataType();
            var rhs = RHS.GetDataType();

            if (lhs == DataType.Int && rhs == DataType.Int)
            {
                ncs.Add(new NCSInstruction.LEQII());
            }
            else if (lhs == DataType.Float && rhs == DataType.Float)
            {
                ncs.Add(new NCSInstruction.LEQFF());
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
