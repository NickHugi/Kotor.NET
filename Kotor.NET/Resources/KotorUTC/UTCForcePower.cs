using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTC;

public class UTCForcePower
{
    private GFF _source { get; }
    private GFFStruct _powerSource { get; }

    internal UTCForcePower(GFF source, GFFStruct powerSource)
    {
        _source = source;
        _powerSource = powerSource;
    }

    /// <summary>
    /// Index of the force power in the creature's class.
    /// </summary>
    public int Index
    {
        get
        {
            RaiseIfDoesNotExist();
            return _powerSource.GetList("ClassList")!.IndexOf(_powerSource);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Spell</c> field in the UTC and is an index into
    /// the <c>spells.2da</c> file.
    /// </remarks>
    public ushort ForcePowerID
    {
        get => _powerSource.GetUInt16("Spell") ?? 0;
        set => _powerSource.SetUInt16("Spell", value);
    }

    /// <summary>
    /// Removes the force power from the creature's class.
    /// </summary>
    public void Remove()
    {
        RaiseIfDoesNotExist();
        _powerSource.GetList("KnownList0")?.Remove(_powerSource);
    }

    private void RaiseIfDoesNotExist()
    {
        var classList = _powerSource.GetList("KnownList0");
        if (classList is null || !classList.Contains(_powerSource))
        {
            throw new ArgumentException("This class no longer exists.");
        }
    }
}
