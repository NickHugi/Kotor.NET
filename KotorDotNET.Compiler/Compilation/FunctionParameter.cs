﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Compiler.Calculator;
using KotorDotNET.FileFormats.KotorNCS;

namespace KotorDotNET.Compiler.Compilation
{
    public class FunctionParameter : ASTNode
    {
        public DataType DataType { get; }
        public string Symbol { get; }
        public IExpression? Default { get; }

        public FunctionParameter(DataType dataType, string symbol, IExpression? @default)
        {
            DataType = dataType;
            Symbol = symbol;
            Default = @default;
        }

        public void Compile(SymbolTable symbolTable, NCS ncs) => throw new NotImplementedException();
    }
}
