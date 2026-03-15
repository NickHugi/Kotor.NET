using System;
using Silk.NET.Core.Contexts;

namespace Kotor.DevelopmentKit.ViewerMDL;

public class AvaloniaSilkNativeContext : INativeContext
{
    private readonly Func<string, nint> _getProcAddress;

    public AvaloniaSilkNativeContext(Func<string, nint> getProcAddress)
    {
        _getProcAddress = getProcAddress;
    }

    public void Dispose()
    {

    }

    public nint GetProcAddress(string proc, int? slot = null)
    {
        return _getProcAddress(proc);
    }

    public bool TryGetProcAddress(string proc, out nint addr, int? slot = null)
    {
        addr = _getProcAddress(proc);
        return true;
    }
}
