using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryERF;
using Kotor.NET.Formats.BinaryERF.Serialisation;
using Kotor.NET.Formats.BinaryRIM.Serialisation;
using Kotor.NET.Resources.KotorERF;
using Kotor.NET.Resources.KotorRIM;
using ReactiveUI;
using SkiaSharp;

namespace Kotor.DevelopmentKit.EditorERF.ViewModels;

public class ERFViewModel : ReactiveObject
{
    public ObservableCollection<ERFResource> Resources { get; } = new();

    public void Load(ERF model)
    {
        Resources.Clear();

        foreach (var resource in model)
        {
            Resources.Add(new ERFResource
            {
                ResRef = resource.ResRef.Get(),
                ResourceType = resource.Type,
                Data = resource.Data,
            });
        }
    }
    public void Load(RIM model)
    {
        Resources.Clear();

        foreach (var resource in model)
        {
            Resources.Add(new ERFResource
            {
                ResRef = resource.ResRef.Get(),
                ResourceType = resource.Type,
                Data = resource.Data,
            });
        }
    }

    public ERF BuildERF()
    {
        var erf = new ERF();
        Resources.ToList().ForEach(x => erf.Add(x.ResRef, x.ResourceType, x.Data));
        return erf;
    }
    public RIM BuildRIM()
    {
        var rim = new RIM();
        Resources.ToList().ForEach(x => rim.Add(x.ResRef, x.ResourceType, x.Data));
        return rim;
    }

    public void Deserialize(ResourceType resourceType, Stream stream)
    {
        if (resourceType == ResourceType.ERF || resourceType == ResourceType.MOD)
        {
            var erf = ERF.FromStream(stream);
            Load(erf);
        }
        else if (resourceType == ResourceType.RIM)
        {
            var rim = RIM.FromStream(stream);
            Load(rim);
        }
        else
        {
            throw new NotImplementedException();
        }
    }
    public void Deserialize(ResourceType resourceType, byte[] data)
    {
        using var memoryStream = new MemoryStream(data);
        Deserialize(resourceType, memoryStream);
    }

    public void Serialize(ResourceType resourceType, Stream stream)
    {
        if (resourceType == ResourceType.ERF || resourceType == ResourceType.MOD)
        {
            new ERFBinarySerializer(BuildERF()).Serialize().Write(stream);
        }
        else if (resourceType == ResourceType.RIM)
        {
            new RIMBinarySerializer(BuildRIM()).Serialize().Write(stream);
        }
        else
        {
            throw new NotImplementedException();
        }
    }
    public byte[] Serialize(ResourceType resourceType)
    {
        using var memoryStream = new MemoryStream();
        Serialize(resourceType, memoryStream);
        return memoryStream.ToArray();
    }

    public void AddOrOverrideResource(ERFResource resource)
    {
        var index = Resources.ToList().FindIndex(x => x.ResRef == resource.ResRef && x.ResourceType == resource.ResourceType);
        if (index == -1)
        {
            Resources.Add(resource);
        }
        else
        {
            Resources.RemoveAt(index);
            Resources.Insert(index, resource);
        }
    }
}

public class ERFResource
{
    public required string ResRef { get; set; }
    public required ResourceType ResourceType { get; set; }
    public required byte[] Data { get; set; }
}
