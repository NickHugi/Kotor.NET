using System.Collections.Generic;
using Kotor.NET.Common;

namespace Kotor.DevelopmentKit.Base.Settings.Interfaces;

public interface IInstallationLocator
{
    public IEnumerable<PotentialGameDirectory> Search();
}
