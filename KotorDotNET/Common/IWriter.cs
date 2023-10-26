using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Common
{
    public interface IWriter<T>
    {
        //byte[] Bytes();

        void Write(T value);
    }
}
