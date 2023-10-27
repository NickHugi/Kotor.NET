using KotorDotNET.FileFormats.Kotor2DA;
using KotorDotNET.FileFormats.KotorGFF;
using KotorDotNET.ResourceContainers;

namespace KotorDotNET.Patching;

public class Patcher
{
    public Patcher(IMemory memory, ILogger logger, PatcherData data)
    {
        Memory = memory;
        Logger = logger;
        PatcherData = data;
    }

    public IMemory Memory { get; set; }
    public ILogger Logger { get; set; }
    public PatcherData PatcherData { get; set; }

    /// <summary>
    ///     Execute the patcher data to the game files.
    /// </summary>
    public void Run()
    {

    }
}
