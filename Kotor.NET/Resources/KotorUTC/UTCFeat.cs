using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTC;

public class UTCFeat
{
    private GFF _source { get; }
    private GFFStruct _featSource { get; }
    private GFFList? _featList => _source.Root.GetList("FeatList");

    internal UTCFeat(GFF source, GFFStruct featSource)
    {
        _source = source;
        _featSource = featSource;
    }

    /// <summary>
    /// Index of the feat on the assigned creature.
    /// </summary>
    public int Index
    {
        get
        {
            RaiseIfDoesNotExist();
            return _featList!.IndexOf(_featSource);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Feat</c> field.
    /// </remarks>
    public ushort FeatID
    {
        get => _featSource.GetUInt16("Feat") ?? 0;
        set => _featSource.SetUInt16("Feat", value);
    }

    /// <summary>
    /// Removes the feat from the creature.
    /// </summary>
    public void Remove()
    {
        var index = Index;

        RaiseIfDoesNotExist();
        _featList!.Remove(_featSource);
    }

    /// <summary>
    /// Returns true if the item still exists in the creature's inventory.
    /// </summary>
    public bool Exists()
    {
        var featList = _featList;
        return featList is null || !featList.Contains(_featSource);
    }

    private void RaiseIfDoesNotExist()
    {
        if (Exists())
        {
            throw new ArgumentException("This feat no longer exists.");
        }
    }
}
