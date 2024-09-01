using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinarySSF;

namespace Kotor.NET.Tests.Formats.BinarySSF;

public class TestSSFDeserializer
{
    public static readonly string File1Filepath = "Formats/BinarySSF/file1.ssf";

    [Fact]
    public void Test_ReadFile1()
    {
        using var stream = File.OpenRead(File1Filepath);
        var binary = new SSFBinary(stream);
        var deserializer = new SSFBinaryDeserializer(binary);
        var ssf = deserializer.Deserialize();
        
        Assert.Equal(123075u, ssf.Battlecry1);
        Assert.Equal(123074u, ssf.Battlecry2);
        Assert.Equal(123073u, ssf.Battlecry3);
        Assert.Equal(123072u, ssf.Battlecry4);
        Assert.Equal(123071u, ssf.Battlecry5);
        Assert.Equal(123070u, ssf.Select1);
        Assert.Equal(123069u, ssf.Select2);
        Assert.Equal(123068u, ssf.Select3);
        Assert.Equal(123067u, ssf.AttackGrunt1);
        Assert.Equal(123066u, ssf.AttackGrunt2);
        Assert.Equal(123065u, ssf.AttackGrunt3);
        Assert.Equal(123064u, ssf.PainGrunt1);
        Assert.Equal(123063u, ssf.PainGrunt2);
        Assert.Equal(123062u, ssf.LowHealth);
        Assert.Equal(123061u, ssf.Dead);
        Assert.Equal(123060u, ssf.CriticalHit);
        Assert.Equal(123059u, ssf.TargetImmune);
        Assert.Equal(123058u, ssf.LayMine);
        Assert.Equal(123057u, ssf.DisarmMine);
        Assert.Equal(123056u, ssf.BeginStealth);
        Assert.Equal(123055u, ssf.BeginSearch);
        Assert.Equal(123054u, ssf.BeginUnlock);
        Assert.Equal(123053u, ssf.UnlockFailed);
        Assert.Equal(123052u, ssf.UnlockSuccess);
        Assert.Equal(123051u, ssf.PartySeparated);
        Assert.Equal(123050u, ssf.PartyRejoined);
        Assert.Equal(123049u, ssf.Poisoned);
    }
}
