﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Data
{
    public class LocalizedString
    {
        public int StringRef { get; set; }
        public List<LocalizedSubstring> Strings { get; set; }
    }
}
