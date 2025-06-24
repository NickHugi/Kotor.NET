using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using Kotor.NET.Helpers;

namespace Kotor.DevelopmentKit.Base.Settings.Services;

public interface ISearchForInstallationService
{
    public IEnumerable<PotentialGameDirectory> Search();
}

public class SearchForInstallationService : ISearchForInstallationService
{
    public IEnumerable<PotentialGameDirectory> Search()
    {
        return GameDirectoryLocator.Instance.Locate();
    }
}
