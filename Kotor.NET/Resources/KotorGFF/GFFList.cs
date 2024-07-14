using System.Collections;

namespace Kotor.NET.Resources.KotorGFF;

public class GFFList : IList<GFFStruct>
{
    private List<GFFStruct> _structs = new();

    public GFFStruct this[int index]
    {
        get => _structs[index];
        set => _structs[index] = value;
    }

    public int Count => _structs.Count;
    public bool IsReadOnly => false;

    public GFFStruct Add(uint structID = 0)
    {
        var gffStruct = new GFFStruct(structID);
        Add(gffStruct);
        return gffStruct;
    }
    public void Add(GFFStruct item) => _structs.Add(item);
    public void Clear() => _structs.Clear();
    public bool Contains(GFFStruct item) => _structs.Contains(item);
    public void CopyTo(GFFStruct[] array, int arrayIndex) => _structs.CopyTo(array, arrayIndex);
    public IEnumerator<GFFStruct> GetEnumerator() => _structs.GetEnumerator();
    public int IndexOf(GFFStruct item) => _structs.IndexOf(item);
    public void Insert(int index, GFFStruct item) => _structs.Insert(index, item);
    public bool Remove(GFFStruct item) => _structs.Remove(item);
    public void RemoveAt(int index) => _structs.RemoveAt(index);
    IEnumerator IEnumerable.GetEnumerator() => _structs.GetEnumerator();

    public override string ToString()
    {
        return "Count=" + _structs.Count();
    }
}
