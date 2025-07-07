using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Interfaces;
using Kotor.NET.Common;
using Kotor.NET.Helpers;

namespace Kotor.DevelopmentKit.Base.Settings.Services;

public class InstallationLocator : IInstallationLocator
{
    public IEnumerable<PotentialGameDirectory> Search()
    {
        return GameDirectoryLocator.Instance.Locate();
    }
}
