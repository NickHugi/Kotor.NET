using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Interfaces;
using Kotor.DevelopmentKit.Base.Settings.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kotor.DevelopmentKit.Base.Settings.Services;

public class SaveSettingsService : ISaveSettingsService
{
    public void Save(string filepath, DefaultSettingsRoot root)
    {
        if (File.Exists(filepath))
        {
            var oldData = File.ReadAllText(filepath);
            var oldJson = JObject.Parse(oldData);

            var newJson = JObject.FromObject(root);
            oldJson.Merge(newJson, new JsonMergeSettings()
            {
                MergeArrayHandling = MergeArrayHandling.Replace,
                MergeNullValueHandling = MergeNullValueHandling.Merge,
                PropertyNameComparison = StringComparison.CurrentCultureIgnoreCase,
            });

            var newData = JsonConvert.SerializeObject(oldJson);
            File.WriteAllText(filepath, newData);
        }
        else
        {
            var newJson = JObject.FromObject(root);
            var newData = JsonConvert.SerializeObject(newJson);
            File.WriteAllText(filepath, newData);
        }
    }
}
