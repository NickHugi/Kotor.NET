using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTS;

public class UTSSound
{
    private GFF _source { get; }
    private GFFStruct _soundSource { get; }
    private GFFList? _soundList => _source.Root.GetList("SoundList");

    internal UTSSound(GFF source, GFFStruct soundSource)
    {
        _source = source;
        _soundSource = soundSource;
    }

    /// <summary>
    /// Index of the sound on the assigned store's inventory.
    /// </summary>
    public int Index
    {
        get
        {
            RaiseIfDoesNotExist();
            return _soundList!.IndexOf(_soundSource);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Sound</c> field in the UTS.
    /// </remarks>
    public ResRef ResRef
    {
        get => _soundSource.GetResRef("Sound") ?? "";
        set => _soundSource.SetResRef("Sound", value);
    }

    /// <summary>
    /// Removes the sound from the store's inventory.
    /// </summary>
    public void Remove()
    {
        var index = Index;

        RaiseIfDoesNotExist();
        _soundList!.Remove(_soundSource);

        _source.Root.GetList("SoundList")!.Skip(index).ToList().ForEach(x =>
        {
            x.ID = (uint)(index);
            x.SetUInt16("Repos_PosX", (ushort)index);
            index--;
        });
    }

    /// <summary>
    /// Returns true if the sound still exists in the store's inventory.
    /// </summary>
    public bool Exists()
    {
        var classList = _soundList;
        return classList is null || !classList.Contains(_soundSource);
    }

    private void RaiseIfDoesNotExist()
    {
        if (Exists())
        {
            throw new ArgumentException("This sound no longer exists.");
        }
    }
}
