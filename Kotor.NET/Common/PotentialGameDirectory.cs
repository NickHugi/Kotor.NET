using System.Diagnostics.CodeAnalysis;

namespace Kotor.NET.Common;

public class PotentialGameDirectory
{
    public string Path { get; }
    public Release Release { get; }
    public Platform Platform { get; }
    public GameEngine Game { get; }

    public PotentialGameDirectory(Platform platform, GameEngine game, Release release, string path)
    {
        Path = path;
        Release = release;
        Platform = platform;
        Game = game;
    }
}
