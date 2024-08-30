using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorARE;

public class ARERoom
{
    private GFF _source { get; }
    private GFFStruct _roomSource { get; }
    private GFFList? _roomList => _source.Root.GetList("Rooms");

    internal ARERoom(GFF source, GFFStruct roomSource)
    {
        _source = source;
        _roomSource = roomSource;
    }

    /// <summary>
    /// Index of the room on the room in the list.
    /// </summary>
    public int Index
    {
        get
        {
            RaiseIfDoesNotExist();
            return _roomList!.IndexOf(_roomSource);
        }
    }

    /// <summary>
    /// Returns true if the room still exists.
    /// </summary>
    public bool Exists
    {
        get => _roomList is not null && _roomList.Contains(_roomSource);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>RoomName</c> field.
    /// </remarks>
    public string RoomName
    {
        get => _roomSource.GetString("RoomName") ?? "";
        set => _roomSource.SetString("RoomName", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ForceRating</c> field.
    /// </remarks>
    public int ForceRating
    {
        get => _roomSource.GetInt32("ForceRating") ?? 0;
        set => _roomSource.SetInt32("ForceRating", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>EnvAudio</c> field.
    /// </remarks>
    public int EnvironmentAudio
    {
        get => _roomSource.GetInt32("EnvAudio") ?? 0;
        set => _roomSource.SetInt32("EnvAudio", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>AmbientScale</c> field.
    /// </remarks>
    public float AmbientScale
    {
        get => _roomSource.GetSingle("AmbientScale") ?? 0;
        set => _roomSource.SetSingle("AmbientScale", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>DisableWeather</c> field. Only supported by KotOR 2.
    /// </remarks>
    public bool DisableWeather
    {
        get => (_roomSource.GetUInt8("DisableWeather") ?? 0) != 0;
        set => _roomSource.SetUInt8("DisableWeather", Convert.ToByte(value));
    }

    /// <summary>
    /// Removes the room.
    /// </summary>
    public void Remove()
    {
        RaiseIfDoesNotExist();
        _roomList!.Remove(_roomSource);
    }

    private void RaiseIfDoesNotExist()
    {
        if (!Exists)
        {
            throw new ArgumentException("This room no longer exists.");
        }
    }
}
