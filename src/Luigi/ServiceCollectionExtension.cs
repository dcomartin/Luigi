using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Luigi
{
    public static class ServiceCollectionExtension
    {
        public static void AddLuigi(this IServiceCollection serviceCollection, params Assembly[] assemblies)
        {
            serviceCollection.AddTransient<IDispatcher, Dispatcher>();
            
            serviceCollection.Scan(scan =>
            {
                if (assemblies?.Length > 0)
                {
                    scan.FromAssemblies(assemblies).AddLuigiTypes();
                }
                else
                {
                    scan.FromEntryAssembly().AddLuigiTypes();
                }
            });
        }

        private static void AddLuigiTypes(this IImplementationTypeSelector selector)
        {
            selector
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryPipeline<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryPipe<,>)))
                .AsSelf()
                .WithTransientLifetime()
            
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryPipeline<,,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryPipe<,,>)))
                .AsSelf()
                .WithTransientLifetime()
                
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandPipeline<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandPipe<,>)))
                .AsSelf()
                .WithTransientLifetime()
            
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandPipeline<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandPipe<>)))
                .AsSelf()
                .WithTransientLifetime()
                
                .AddClasses(classes => classes.AssignableTo(typeof(IEventPipeline<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IEventPipe<,>)))
                .AsSelf()
                .WithTransientLifetime()
            
                .AddClasses(classes => classes.AssignableTo(typeof(IEventPipeline<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IEventPipe<>)))
                .AsSelf()
                .WithTransientLifetime();
            
            /*
            selector
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryPipeline<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryPipeline<,,>)))
                    .AsSelf()
                    .WithTransientLifetime()
            
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryPipe<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryPipe<,,>)))
                    .AsSelf()
                    .WithTransientLifetime()
                
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandPipeline<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandPipeline<,>)))
                    .AsSelf()
                    .WithTransientLifetime()
            
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandPipe<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandPipe<,>)))
                    .AsSelf()
                    .WithTransientLifetime();*/
        }
    }
}