using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;

namespace Kotor.NET.Helpers;

public class GameDirectoryLocator
{
    public static ImmutableArray<PotentialGameDirectory> Locations { get; } =
    [
        new(Platform.Windows, GameEngine.K1, Release.Steam, @"C:\Program Files\Steam\steamapps\common\swkotor"),
        new(Platform.Windows, GameEngine.K1, Release.Steam, @"C:\Program Files (x86)\Steam\steamapps\common\swkotor"),
        new(Platform.Windows, GameEngine.K1, Release.Disc, @"C:\Program Files\LucasArts\SWKotOR"),
        new(Platform.Windows, GameEngine.K1, Release.Disc, @"C:\Program Files (x86)\LucasArts\SWKotOR"),
        new(Platform.Windows, GameEngine.K1, Release.GOG, @"C:\GOG Games\Star Wars - KotOR"),
        new(Platform.Windows, GameEngine.K1, Release.Amazon, @"C:\Amazon Games\Library\Star Wars - Knights of the Old"),
        new(Platform.Windows, GameEngine.K2, Release.Steam, @"C:\Program Files\Steam\steamapps\common\Knights of the Old Republic II"),
        new(Platform.Windows, GameEngine.K2, Release.Steam, @"C:\Program Files (x86)\Steam\steamapps\common\Knights of the Old Republic II"),
        new(Platform.Windows, GameEngine.K2, Release.Disc, @"C:\Program Files\LucasArts\SWKotOR2"),
        new(Platform.Windows, GameEngine.K2, Release.Disc, @"C:\Program Files (x86)\LucasArts\SWKotOR2"),
        new(Platform.Windows, GameEngine.K2, Release.GOG, @"C:\GOG Games\Star Wars - KotOR2"),
        new(Platform.Mac, GameEngine.K1, Release.Steam, @"~/Library/Application Support/Steam/steamapps/common/swkotor/Knights of the Old Republic.app/Contents/Assets"),
        new(Platform.Mac, GameEngine.K1, Release.Steam, @"~/Library/Applications/Steam/steamapps/common/swkotor/Knights of the Old Republic.app/Contents/Assets/"),
        new(Platform.Mac, GameEngine.K2, Release.Steam, @"~/Library/Application Support/Steam/steamapps/common/Knights of the Old Republic II/Knights of the Old Republic II.app/Contents/Assets"),
        new(Platform.Mac, GameEngine.K2, Release.Steam, @"~/Library/Applications/Steam/steamapps/common/Knights of the Old Republic II/Star Wars™: Knights of the Old Republic II.app/Contents/GameData"),
        new(Platform.Mac, GameEngine.K2, Release.Steam, @"~/Library/Application Support/Steam/steamapps/common/Knights of the Old Republic II/KOTOR2.app/Contents/GameData/"),
        new(Platform.Linux, GameEngine.K1, Release.Steam, @"~/.local/share/steam/common/steamapps/swkotor"),
        new(Platform.Linux, GameEngine.K1, Release.Steam, @"~/.local/share/steam/common/swkotor"),
        new(Platform.Linux, GameEngine.K1, Release.Steam, @"~/.steam/debian-installation/steamapps/common/swkotor"),
        new(Platform.Linux, GameEngine.K1, Release.Steam, @"~/.steam/root/steamapps/common/swkotor"),
        new(Platform.Linux, GameEngine.K1, Release.Steam, @"~/.var/app/com.valvesoftware.Steam/.local/share/Steam/steamapps/common/swkotor"),
        new(Platform.Linux, GameEngine.K1, Release.Steam, @"/mnt/C/Program Files/Steam/steamapps/common/swkotor"),
        new(Platform.Linux, GameEngine.K1, Release.Steam, @"/mnt/C/Program Files (x86)/Steam/steamapps/common/swkotor"),
        new(Platform.Linux, GameEngine.K1, Release.Disc, @"/mnt/C/Program Files/LucasArts/SWKotOR"),
        new(Platform.Linux, GameEngine.K1, Release.Disc, @"/mnt/C/Program Files (x86)/LucasArts/SWKotOR"),
        new(Platform.Linux, GameEngine.K1, Release.GOG, @"/mnt/C/GOG Games/Star Wars - KotOR"),
        new(Platform.Linux, GameEngine.K1, Release.Amazon, @"/mnt/C/Amazon Games/Library/Star Wars - Knights of the Old"),
        new(Platform.Linux, GameEngine.K2, Release.Steam, @"~/.local/share/Steam/common/steamapps/Knights of the Old Republic II"),
        new(Platform.Linux, GameEngine.K2, Release.Steam, @"~/.local/share/Steam/common/steamapps/kotor2"),
        new(Platform.Linux, GameEngine.K2, Release.Steam, @"~/.local/share/aspyr-media/kotor2"),
        new(Platform.Linux, GameEngine.K2, Release.Steam, @"~/.local/share/aspyr-media/Knights of the Old Republic II"),
        new(Platform.Linux, GameEngine.K2, Release.Steam, @"~/.local/share/Steam/common/Knights of the Old Republic II"),
        new(Platform.Linux, GameEngine.K2, Release.Steam, @"~/.steam/debian-installation/steamapps/common/Knights of the Old Republic II"),
        new(Platform.Linux, GameEngine.K2, Release.Steam, @"~/.steam/debian-installation/steamapps/common/kotor2"),
        new(Platform.Linux, GameEngine.K2, Release.Steam, @"~/.steam/root/steamapps/common/Knights of the Old Republic II"),
        new(Platform.Linux, GameEngine.K2, Release.Steam, @"~/.var/app/com.valvesoftware.Steam/.local/share/Steam/steamapps/common/Knights of the Old Republic II/steamassets"),
        new(Platform.Linux, GameEngine.K2, Release.Steam, @"/mnt/C/Program Files/Steam/steamapps/common/Knights of the Old Republic II"),
        new(Platform.Linux, GameEngine.K2, Release.Steam, @"/mnt/C/Program Files (x86)/Steam/steamapps/common/Knights of the Old Republic II"),
        new(Platform.Linux, GameEngine.K2, Release.Disc, @"/mnt/C/Program Files/LucasArts/SWKotOR2"),
        new(Platform.Linux, GameEngine.K2, Release.Disc, @"/mnt/C/Program Files (x86)/LucasArts/SWKotOR2"),
        new(Platform.Linux, GameEngine.K2, Release.GOG, @"/mnt/C/GOG Games/Star Wars - KotOR2"),

    ];

    public static GameDirectoryLocator Instance { get; } = new();

    public PotentialGameDirectory[] Locate()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return LocateOnWindows();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return LocateOnMac();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return LocateOnLinux();
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    private PotentialGameDirectory[] LocateOnWindows()
    {
        return Locations
            .Where(x => x.Platform == Platform.Windows)
            .Where(x => Path.Exists(x.Path))
            .ToArray();
    }
    private PotentialGameDirectory[] LocateOnMac()
    {
        return Locations
            .Where(x => x.Platform == Platform.Windows)
            .Where(x => Path.Exists(x.Path))
            .ToArray();
    }
    private PotentialGameDirectory[] LocateOnLinux()
    {
        return Locations
            .Where(x => x.Platform == Platform.Windows)
            .Where(x => Path.Exists(x.Path))
            .ToArray();
    }
}
