using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kotor.DevelopmentKit.Base.Settings.Services;

public interface ILoadSettingsService
{
    public T Load<T>(string filepath) where T : DefaultSettingsRoot, new();

    DefaultSettingsRoot Load(string filepath, Type type);
}

public class LoadSettingsService : ILoadSettingsService
{
    public T Load<T>(string filepath)
        where T : DefaultSettingsRoot, new()
    {
        if (File.Exists(filepath))
        {
            var data = File.ReadAllText(filepath);
            var settings = JsonConvert.DeserializeObject<T>(data);
            return settings;
        }
        else
        {
            return new T();
        }
    }

    public DefaultSettingsRoot Load(string filepath, Type type)
    {
        if (type.IsAssignableFrom(typeof(DefaultSettingsRoot)))
            throw new ArgumentException(); // TODO

        if (File.Exists(filepath))
        {
            var data = File.ReadAllText(filepath);
            var settings = JsonConvert.DeserializeObject(data, type);
            return (DefaultSettingsRoot)settings;
        }
        else
        {
            return (DefaultSettingsRoot)Activator.CreateInstance(type);
        }
    }
}
