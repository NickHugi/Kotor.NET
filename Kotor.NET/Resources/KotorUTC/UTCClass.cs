using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTC;

public class UTCClass
{
    private GFF _source { get; }
    private GFFStruct _classSource { get; }

    internal UTCClass(GFF source, GFFStruct classSource)
    {
        _source = source;
        _classSource = classSource;
    }

    /// <summary>
    /// Index of the class on the assigned creature.
    /// </summary>
    public int Index
    {
        get
        {
            RaiseIfDoesNotExist();
            return _source.Root.GetList("ClassList")!.IndexOf(_classSource);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Class</c> field in the UTC and is an index into
    /// the <c>classes.2da</c> file.
    /// </remarks>
    public int ClassID
    {
        get => _classSource.GetInt32("Class") ?? 0;
        set => _classSource.SetInt32("Class", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ClassLevel</c> field.
    /// </remarks>
    public short Level
    {
        get => _classSource.GetInt16("ClassLevel") ?? 0;
        set => _classSource.SetInt16("ClassLevel", value);
    }

    public UTCForcePowerCollection ForcePowers => new(_source, _classSource);

    /// <summary>
    /// Removes the class from the creature.
    /// </summary>
    public void Remove()
    {
        RaiseIfDoesNotExist();
        _source.Root.GetList("ClassList")?.Remove(_classSource);
    }

    /// <summary>
    /// Returns true if the class still exists on the creature.
    /// </summary>
    public bool Exists()
    {
        var classList = _source.Root.GetList("ClassList");
        return classList is null || !classList.Contains(_classSource);
    }

    private void RaiseIfDoesNotExist()
    {
        if (Exists())
        {
            throw new ArgumentException("This class no longer exists.");
        }
    }
}
