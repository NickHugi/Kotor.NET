using Kotor.DevelopmentKit.Base.Settings.Values;

namespace Kotor.DevelopmentKit.Base.Settings.Interfaces;

public interface ISaveSettingsService
{
    public void Save(string filepath, DefaultSettingsRoot root);
}
