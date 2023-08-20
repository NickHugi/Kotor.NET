// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.FileFormats.Kotor2DA;
using KotorDotNET.Patching.Diff;

namespace KotorDotNET.Tests.Tests.Patching.Diff
{
    [TestClass]
    public class TestDiff2DA
    {
        [TestMethod]
        public void IsSame_ColumnCountMismatch()
        {
            var older = new TwoDA();
            older.AddColumn("ABC");
            older.AddColumn("123");

            var newer = new TwoDA();
            newer.AddColumn("ABC");

            var diff = new Diff2DA(older, newer);

            // Columns do not match so the 2DAs are not the same.
            Assert.IsFalse(diff.IsSame());
        }

        [TestMethod]
        public void IsSame_RowCountMismatch()
        {
            var older = new TwoDA();
            older.AddColumn("A");
            older.AddColumn("B");
            older.AddRow("1");
            older.AddRow("2");

            var newer = new TwoDA();
            newer.AddColumn("A");
            newer.AddColumn("B");
            newer.AddRow("1");

            var diff = new Diff2DA(older, newer);

            // One has more rows then the others so the 2DAs are not the same.
            Assert.IsFalse(diff.IsSame());
        }

        [TestMethod]
        public void IsSame_CellMismatch()
        {
            var older = new TwoDA();
            older.AddColumn("A");
            older.AddColumn("B");
            older.AddRow("1");
            older.AddRow("2");

            var newer = new TwoDA();
            newer.AddColumn("A");
            newer.AddColumn("B");
            newer.AddRow("1");
            newer.AddRow("2");
            newer.Row(0).SetCell("A", "asdf");

            var diff = new Diff2DA(older, newer);

            // One has more rows then the others so the 2DAs are not the same.
            Assert.IsFalse(diff.IsSame());
        }

        [TestMethod]
        public void IsSame_AreMatching()
        {
            var older = new TwoDA();
            older.AddColumn("A");
            older.AddColumn("B");
            older.AddRow("1");
            older.AddRow("2");
            older.Row(0).SetCell("A", "asdf");

            var newer = new TwoDA();
            newer.AddColumn("A");
            newer.AddColumn("B");
            newer.AddRow("1");
            newer.AddRow("2");
            newer.Row(0).SetCell("A", "asdf");

            var diff = new Diff2DA(older, newer);

            // One has more rows then the others so the 2DAs are not the same.
            Assert.IsTrue(diff.IsSame());
        }
    }
}
