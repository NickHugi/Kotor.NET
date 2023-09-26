// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Compiler;

namespace KotorDotNET.Tests.Tests
{
    [TestClass]
    public class testz
    {
        [TestMethod]
        public void GOGOGO()
        {
            try
            {

                var compiler = new NSSCompiler();
                //compiler.Compile(@"#include ""a""");
                compiler.Compile(@"int wat(int yez);");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
