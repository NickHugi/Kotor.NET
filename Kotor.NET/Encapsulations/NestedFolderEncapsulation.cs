using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.NET.Encapsulations;

public class NestedFolderEncapsulation : IEncapsulation
{
    public string Path => throw new NotImplementedException();

    public int Count => throw new NotImplementedException();

    public void Delete(string resref, ResourceType type) => throw new NotImplementedException();
    public ResourceInfo Find(string resref, ResourceType type) => throw new NotImplementedException();
    public IEnumerator<ResourceInfo> GetEnumerator() => throw new NotImplementedException();
    public byte[] Read(string resref, ResourceType type) => throw new NotImplementedException();
    public void Reload() => throw new NotImplementedException();
    public void Write(string resref, ResourceType type, byte[] data) => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
