using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Interfaces;
using Kotor.DevelopmentKit.Base.Settings.Services;
using Kotor.DevelopmentKit.Base.Settings.Values;
using Kotor.DevelopmentKit.Base.Settings.ViewModels;
using Kotor.NET.Interfaces;
using Kotor.NET.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Kotor.DevelopmentKit.Base.Extensions;

public static class ServiceCollectionExtensions
{
    public static ServiceCollection AddBaseServices(this ServiceCollection services)
    {
        services.AddScoped<SettingsDialogViewModel>();
        services.AddScoped<IInstallationsScanner, InstallationsScanner>();
        services.AddScoped<IInstallationLocator, InstallationLocator>();
        services.AddScoped<ISettingsImporter, SettingsImporter>();
        services.AddScoped<ISettingsExporter, SettingsExporter>();
        services.AddScoped<ITalkTableLookup, TalkTableLookup>();

        return services;
    }

    public static ServiceCollection AddSettings<TSettings>(this ServiceCollection services)
        where TSettings : DefaultSettingsRoot, new()
    {
        services.AddSingleton<DefaultSettingsRoot, TSettings>(x =>
        {
            return x.GetService<ISettingsImporter>()!.Load<TSettings>(DefaultSettingsRoot.SettingsFilepath);
        });

        return services;
    }
}
