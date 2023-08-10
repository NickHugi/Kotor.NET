using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Extensions
{
    public static class String
    {
        public static string Truncate(this string value, int length)
        {
            var count = (value.Length < length) ? value.Length : length;
            return new string(value.ToList().GetRange(0, count).ToArray());
        }
    }
}
