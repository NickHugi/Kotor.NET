using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.EditorGFF.Interfaces;
using Kotor.DevelopmentKit.EditorGFF.Services;
using Kotor.DevelopmentKit.EditorGFF.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Kotor.DevelopmentKit.EditorGFF.Extensions;

public static class ServiceCollectionExtensions
{
    public static ServiceCollection AddGFFEditorServices(this ServiceCollection services)
    {
        services.AddScoped<GFFResourceEditorViewModel>();
        services.AddTransient<INodeSerializer, NodeSerializer>();
        services.AddTransient<INodeDeserializer, NodeDeserializer>();

        return services;
    }
}
