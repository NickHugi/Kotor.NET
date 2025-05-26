using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinarySFF.Serialisation;
using Kotor.NET.Formats.BinarySSF;

namespace Kotor.NET.Resources.KotorSSF;

public class SSF
{
    public static SSF FromFile(string filepath)
    {
        using var stream = File.OpenRead(filepath);
        return FromStream(stream);
    }
    public static SSF FromBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return FromStream(stream);
    }
    public static SSF FromStream(Stream stream)
    {
        var binary = new SSFBinary(stream);
        var deserializer = new SSFBinaryDeserializer(binary);
        return deserializer.Deserialize();
    }

    public uint Battlecry1 { get; set; } = 0xFFFFFFFF;
    public uint Battlecry2 { get; set; } = 0xFFFFFFFF;
    public uint Battlecry3 { get; set; } = 0xFFFFFFFF;
    public uint Battlecry4 { get; set; } = 0xFFFFFFFF;
    public uint Battlecry5 { get; set; } = 0xFFFFFFFF;
    public uint Battlecry6 { get; set; } = 0xFFFFFFFF;
    public uint Select1 { get; set; } = 0xFFFFFFFF;
    public uint Select2 { get; set; } = 0xFFFFFFFF;
    public uint Select3 { get; set; } = 0xFFFFFFFF;   
    public uint AttackGrunt1 { get; set; } = 0xFFFFFFFF;
    public uint AttackGrunt2 { get; set; } = 0xFFFFFFFF;
    public uint AttackGrunt3 { get; set; } = 0xFFFFFFFF;
    public uint PainGrunt1 { get; set; } = 0xFFFFFFFF;
    public uint PainGrunt2 { get; set; } = 0xFFFFFFFF;
    public uint LowHealth { get; set; } = 0xFFFFFFFF;
    public uint Dead { get; set; } = 0xFFFFFFFF;
    public uint CriticalHit { get; set; } = 0xFFFFFFFF;
    public uint TargetImmune { get; set; } = 0xFFFFFFFF;
    public uint LayMine { get; set; } = 0xFFFFFFFF;
    public uint DisarmMine { get; set; } = 0xFFFFFFFF;
    public uint BeginStealth { get; set; } = 0xFFFFFFFF;
    public uint BeginSearch { get; set; } = 0xFFFFFFFF;
    public uint BeginUnlock { get; set; } = 0xFFFFFFFF;
    public uint UnlockFailed { get; set; } = 0xFFFFFFFF;
    public uint UnlockSuccess { get; set; } = 0xFFFFFFFF;
    public uint PartySeparated { get; set; } = 0xFFFFFFFF;
    public uint PartyRejoined { get; set; } = 0xFFFFFFFF;
    public uint Poisoned { get; set; } = 0xFFFFFFFF;
}
