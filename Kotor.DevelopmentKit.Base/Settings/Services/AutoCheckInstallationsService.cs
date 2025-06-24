using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using Kotor.NET.Helpers;

namespace Kotor.DevelopmentKit.Base.Settings.Services;

public interface IAutoCheckInstallationsService
{
    public void CheckAndAdd();
}

public class AutoCheckInstallationsService
(
    DefaultSettingsRoot settings
) : IAutoCheckInstallationsService
{
    public void CheckAndAdd()
    {
        if (!settings.Common.Installations.AutoCheckForInstallations)
            return;

        var directories = PotentialDirectories();
        directories.ToList().ForEach(potential =>
        {
            var installations = settings.Common.Installations.List;
            var alreadyTaken = installations.Any(existing => String.Equals(
                GetFullPath(potential.Path).TrimEnd('\\'),
                GetFullPath(existing.Path).TrimEnd('\\'),
                StringComparison.InvariantCultureIgnoreCase));

            if (alreadyTaken is false)
            {
                installations.Add(new Installation
                {
                    Name = $"{potential.Game} {potential.Release}",
                    Game = potential.Game,
                    Path = potential.Path,
                    Platform = potential.Platform,
                });
            }
        });
    }

    private IEnumerable<PotentialGameDirectory> PotentialDirectories()
    {
        return GameDirectoryLocator.Instance.Locate();
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
