﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET
{
    internal interface IWriter<T>
    {
        void Write(T value);
    }
}
