using System.Collections.Generic;
using Kotor.NET.Common;

namespace Kotor.DevelopmentKit.Base.Settings.Interfaces;

public interface ISearchForInstallationService
{
    public IEnumerable<PotentialGameDirectory> Search();
}
