using System;
using Kotor.DevelopmentKit.Base.Settings.Values;

namespace Kotor.DevelopmentKit.Base.Settings.Interfaces;

public interface ILoadSettingsService
{
    public T Load<T>(string filepath) where T : DefaultSettingsRoot, new();

    DefaultSettingsRoot Load(string filepath, Type type);
}
