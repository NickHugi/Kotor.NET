using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int length)
        {
            var count = (value.Length < length) ? value.Length : length;
            return new string(value.ToList().GetRange(0, count).ToArray());
        }

        public static string Resize(this string value, int length, char padding = '\0')
        {
            return value.Truncate(32).PadRight(length, padding);
        }

        public static bool EqualsIgnoreCase(this string value1, string value2)
        {
            return StringComparer.CurrentCultureIgnoreCase.Equals(value1, value2);
        }
    }
}
