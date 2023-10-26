// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Common.Creature;
using KotorDotNET.Common.Data;

namespace KotorDotNET.Common.Item
{
    public class EquippedItem
    {
        public ResRef ResRef { get; set; } = "";
        public EquipmentSlot Slot { get; set; }
        public bool Droppable { get; set; }
    }
}
