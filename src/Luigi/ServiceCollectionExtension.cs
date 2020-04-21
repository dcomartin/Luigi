using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Luigi
{
    public static class ServiceCollectionExtension
    {
        public static void AddLuigi(this IServiceCollection serviceCollection, params Assembly[] assemblies)
        {
            serviceCollection.Scan(scan =>
            {
                if (assemblies?.Length > 0)
                {
                    scan.FromAssemblies(assemblies).AddLugiTypes();
                }
                else
                {
                    scan.FromCallingAssembly().AddLugiTypes();
                }
            });
        }

        private static void AddLugiTypes(this IImplementationTypeSelector selector)
        {
            selector.AddClasses(classes => classes.AssignableTo(typeof(IPipeline<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IPipe<,>)))
                .AsSelf()
                .WithTransientLifetime();
        }
    }
}