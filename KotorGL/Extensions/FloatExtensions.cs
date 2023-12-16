using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorGL.Extensions
{
    public static class FloatExtensions
    {
        public static ReadOnlySpan<float> ToReadOnly(this float[] vertices)
        {
            return new ReadOnlySpan<float>(vertices);
        }
    }
}
