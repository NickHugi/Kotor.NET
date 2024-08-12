using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;
using Kotor.NET.Resources.KotorUTE;
using Kotor.NET.Resources.KotorUTM;

namespace Kotor.NET.Resources.KotorUTE;

public class UTECreatureCollection : IEnumerable<UTECreature>
{
    private GFF _source;
    private GFFList? _creatureList => _source.Root.GetList("CreatureList");

    public UTECreatureCollection(GFF source)
    {
        _source = source;
    }

    public UTECreature this[int index]
    {
        get => All().ElementAt(index);
    }

    public int Count => _creatureList?.Count() ?? 0;
    public IEnumerator<UTECreature> GetEnumerator() => All().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => All().GetEnumerator();

    /// <summary>
    /// Adds a new creature to the list.
    /// </summary>
    public UTECreature Add(ResRef resref, int appearanceID, float challengeRating, int guaranteedCount, bool singleSpawn)
    {
        var creatureList = CreateListIfItDoesNotExist();
        var creatureStruct = creatureList.Add((uint)creatureList.Count);

        return new UTECreature(_source, creatureStruct)
        {
            ResRef = resref,
            AppearanceID = appearanceID,
            ChallengeRating = challengeRating,
            GuaranteedCount = guaranteedCount,
            SingleSpawn = singleSpawn,
        };
    }

    /// <summary>
    /// Removes all creatures from the encounter.
    /// </summary>
    public void Clear()
    {
        var creatureList = CreateListIfItDoesNotExist();
        creatureList.Clear();
    }

    private IEnumerable<UTECreature> All()
    {
        return _creatureList?.Select(x => new UTECreature(_source, x)) ?? new List<UTECreature>();
    }
    private GFFList CreateListIfItDoesNotExist()
    {
        return _creatureList ?? _source.Root.SetList("CreatureList");
    }
}
