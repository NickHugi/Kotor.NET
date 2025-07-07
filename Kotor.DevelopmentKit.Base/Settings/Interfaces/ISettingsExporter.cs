using Kotor.DevelopmentKit.Base.Settings.Values;

namespace Kotor.DevelopmentKit.Base.Settings.Interfaces;

public interface ISettingsExporter
{
    public void Save(string filepath, DefaultSettingsRoot root);
}
