using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTE;

public class UTECreature
{
    private GFF _source { get; }
    private GFFStruct _creatureSource { get; }
    private GFFList? _creatureList => _source.Root.GetList("CreatureList");

    internal UTECreature(GFF source, GFFStruct creatureSource)
    {
        _source = source;
        _creatureSource = creatureSource;
    }

    /// <summary>
    /// Index of the creature on the assigned encounter's list.
    /// </summary>
    public int Index
    {
        get
        {
            RaiseIfDoesNotExist();
            return _creatureList!.IndexOf(_creatureSource);
        }
    }

    /// <summary>
    /// Returns true if the creature still exists in the encounter's list.
    /// </summary>
    public bool Exists
    {
        get => _creatureList is not null && _creatureList.Contains(_creatureSource);
    }

    /// <summary>
    /// The creature template that will be spawned.
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ResRef</c> field in the UTE creature struct.
    /// </remarks>
    public ResRef ResRef
    {
        get => _creatureSource.GetResRef("ResRef") ?? "";
        set => _creatureSource.SetResRef("ResRef", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>GuaranteedCount</c> field in the UTE creature struct.
    /// </remarks>
    public int GuaranteedCount
    {
        get => _creatureSource.GetInt32("GuaranteedCount") ?? 0;
        set => _creatureSource.SetInt32("GuaranteedCount", value);
    }

    /// <summary>
    /// This should match the appearance stored in the creature template.
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Appearance</c> field in the UTE creature struct. This is an index into the <c>appearance.2da</c> file.
    /// </remarks>
    public int AppearanceID
    {
        get => _creatureSource.GetInt32("Appearance") ?? 0;
        set => _creatureSource.SetInt32("Appearance", value);
    }

    /// <summary>
    /// This should match the challenge rating stored in the creature template.
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>CR</c> field in the UTE creature struct.
    /// </remarks>
    public float ChallengeRating
    {
        get => _creatureSource.GetSingle("CR") ?? 0;
        set => _creatureSource.SetSingle("CR", value);
    }

    /// <summary>
    /// When true, only a single instance of the creature can spawn at a time within the encounter.
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>SingleSpawn</c> field in the UTE creature struct.
    /// </remarks>
    public bool SingleSpawn
    {
        get => _creatureSource.GetSingle("SingleSpawn") != 0;
        set => _creatureSource.SetSingle("SingleSpawn", Convert.ToByte(value));
    }

    /// <summary>
    /// Removes the creature from the encounters's list.
    /// </summary>
    public void Remove()
    {
        RaiseIfDoesNotExist();
        _creatureList!.Remove(_creatureSource);
    }

    private void RaiseIfDoesNotExist()
    {
        if (!Exists)
            throw new ArgumentException("This creature no longer exists.");
    }
}
