using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.Base.Extensions;

public static class EnumExtensions
{
    public static string GetEnumDescription(this Enum self)
    {
        var description = self.GetType().GetMember(self.ToString()).First().GetCustomAttribute<DescriptionAttribute>()?.Description;
        return description ?? self.ToString();
    }
}
