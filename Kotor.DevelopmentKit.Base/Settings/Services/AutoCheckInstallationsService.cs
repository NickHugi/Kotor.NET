using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Interfaces;
using Kotor.DevelopmentKit.Base.Settings.Values;
using Kotor.NET.Common;
using Kotor.NET.Helpers;

namespace Kotor.DevelopmentKit.Base.Settings.Services;

public class AutoCheckInstallationsService
(
    ISearchForInstallationService _searchForGames,
    DefaultSettingsRoot _settings
) : IAutoCheckInstallationsService
{
    public void CheckAndAdd()
    {
        if (!_settings.Common.Installations.AutoCheckForInstallations)
            return;

        _searchForGames.Search().ToList().ForEach(potential =>
        {
            var installations = _settings.Common.Installations.List;
            var alreadyTaken = installations.Any(existing => String.Equals(
                GetFullPath(potential.Path).TrimEnd('\\'),
                GetFullPath(existing.Path).TrimEnd('\\'),
                StringComparison.InvariantCultureIgnoreCase));

            if (alreadyTaken is false)
            {
                installations.Add(new InstallationSettings
                {
                    Name = $"{potential.Game} {potential.Release}",
                    Game = potential.Game,
                    Path = potential.Path,
                    Platform = potential.Platform,
                });
            }
        });
    }

    private string GetFullPath(string path)
    {
        try
        {
            return Path.GetFullPath(path);
        }
        catch
        {
            return path;
        }
    }
}
