using System.Collections;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorARE;

public class ARERoomCollection : IEnumerable<ARERoom>
{
    private GFF _source;
    private GFFList? _roomList => _source.Root.GetList("Rooms");

    public ARERoomCollection(GFF source)
    {
        _source = source;
    }

    public ARERoom this[int index]
    {
        get => All().ElementAt(index);
    }

    public int Count => _roomList?.Count() ?? 0;
    public IEnumerator<ARERoom> GetEnumerator() => All().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => All().GetEnumerator();

    /// <summary>
    /// Adds a new room.
    /// </summary>
    public ARERoom Add(string roomName, int environmentAudio, int forceRating, float ambientScale, bool disableWeather = false)
    {
        var roomList = CreateListIfItDoesNotExist();
        var roomStruct = roomList.Add(structID: 0);

        return new ARERoom(_source, roomStruct)
        {
            RoomName = roomName,
            EnvironmentAudio = environmentAudio,
            ForceRating = forceRating,
            AmbientScale = ambientScale,
            DisableWeather = disableWeather,
        };
    }

    /// <summary>
    /// Removes all rooms.
    /// </summary>
    public void Clear()
    {
        var roomList = CreateListIfItDoesNotExist();
        roomList.Clear();
    }

    private IEnumerable<ARERoom> All()
    {
        return _roomList?.Select(x => new ARERoom(_source, x)) ?? new List<ARERoom>();
    }
    private GFFList CreateListIfItDoesNotExist()
    {
        return _roomList ?? _source.Root.SetList("Rooms");
    }
}
